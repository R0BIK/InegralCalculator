using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegralCalculator.Model;
using Microsoft.VisualBasic;
using NCalc;
using PCRE;
using WpfMath.Controls;
using Expression = NCalc.Expression;

namespace IntegralCalculator.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _function = "";
    
    [ObservableProperty]
    private string _upperBound = "";
    
    [ObservableProperty]
    private string _lowerBound = "";
    
    [ObservableProperty]
    private string _showIntegral = "";
    
    [ObservableProperty]
    private string _message = "";
    
    [ObservableProperty]
    private string _messageColor = "";

    // private string _mathFunction = "";
    //
    // private string _markdownFunction = "";

    [ObservableProperty]
    private List<string> _results;
    
    [ObservableProperty]
    private List<ulong> _difficulties;
    public ObservableCollection<FormulaControl> Options { get; set; }
    
    public FormulaControl SelectedItem { get; set; }
    
    public ICommand ResolveCommand { get; set; }
    
    public MainViewModel()
    {
        InitComboBox();
        ResolveCommand = new RelayCommand(() =>
        {
            if (Function == "")
            {
                Message = "Введіть функцію інтегралу в текстовому полі!";
                MessageColor = "#eb4034";
            }
            else if (UpperBound == "")
            {
                Message = "Введіть верхню межу інтегралу в текстовому полі!";
                MessageColor = "#eb4034";
                string markdownFunction = ConvertFunctionMarkdown(Function);
                ShowIntegral = @"\int" + "{" + $"{markdownFunction}" + @"\, dx}";
            }
            else if (LowerBound == "")
            {
                Message = "Введіть нижню межу інтегралу в текстовому полі!";
                MessageColor = "#eb4034";
                string markdownFunction = ConvertFunctionMarkdown(Function);
                ShowIntegral = @"\int" + "{" + $"{markdownFunction}" + @"\, dx}";
            }
            else
            {
                SolveIntegral();
                string markdownFunction = ConvertFunctionMarkdown(Function);
                string upperB = ConvertFunctionMarkdown(UpperBound);
                string lowerB = ConvertFunctionMarkdown(LowerBound);
                ShowIntegral = @"\int_{" + $"{lowerB}" + @"}^{" + $"{upperB}" + "}{" + $"{markdownFunction}" + @"\, dx}";
                Console.WriteLine(markdownFunction);
                Console.WriteLine(@"\int_{" + $"{lowerB}" + @"}^{" + $"{upperB}" + "}{" + $"{markdownFunction}" + @"\, dx} =");
            }
        });
    }

    void InitComboBox()
    {
        List<string> avaliableArg = new List<string>
        {
            "x",
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "y",
            "z"
        };
        Options = new ObservableCollection<FormulaControl>();
        for (int i = 0; i < 26; ++i)
        {
            FormulaControl formulaControl = new FormulaControl();
            formulaControl.Formula = avaliableArg[i];
            formulaControl.Scale = 26;
            Options.Add(formulaControl);
        }
        
        SelectedItem = Options[0];
    }

    void SolveIntegral()
    {
        string mathFunction = ConvertFunctionMath(Function);
        string upperB = ConvertFunctionMath(UpperBound);
        string lowerB = ConvertFunctionMath(LowerBound);
        
        Expression function = new Expression(mathFunction);
        function.Parameters["PI"] = Math.PI;
        function.Parameters["E"] = Math.E;

        Expression upper = new Expression(upperB);
        upper.Parameters["PI"] = Math.PI;
        upper.Parameters["E"] = Math.E;
        
        Expression lower = new Expression(lowerB);
        lower.Parameters["PI"] = Math.PI;
        lower.Parameters["E"] = Math.E;
        
        Results = new List<string>
        {
            "",
            "",
            ""
        };
        
        List<string> checkResult = new List<string>();
        List<ulong> difficulty = new List<ulong>();
        
        checkResult.Add(IntegralSolver.SolveBySipmpson(function, upper, lower, SelectedItem.Formula));
        difficulty.Add(IntegralSolver.Dificulty);
        IntegralSolver.Dificulty = 0;
        checkResult.Add(IntegralSolver.SolveByRectangles(function, upper, lower, SelectedItem.Formula));
        difficulty.Add(IntegralSolver.Dificulty);
        IntegralSolver.Dificulty = 0;
        checkResult.Add(IntegralSolver.SolveByTrapezium(function, upper, lower, SelectedItem.Formula));
        difficulty.Add(IntegralSolver.Dificulty);
        IntegralSolver.Dificulty = 0;
        
        int mathErrors = 0;
        int boundErrors = 0;
        int nans = 0;
        Message = "";

        for (int i = 0; i < 3; ++i)
        {
            if (checkResult[i] == "MathError")
            {
                mathErrors++;
                checkResult[i] = "-";
            }
            else if (checkResult[i] == "BoundError")
            {
                checkResult[i] = "-";
                boundErrors++;
            }
            else if (checkResult[i] == "NaN")
            {
                checkResult[i] = "Не має натуральних розв'язків";
                nans++;
            }
        }

        if (mathErrors == 3)
        {
            Message += "Функцію введено не правильно!";
            MessageColor = "#eb4034";
        }
        else if (mathErrors > 0)
        {
            Message += "Деякі методи не змогли зчитати функцію!";
            MessageColor = "#cfa80e";
        }
        else if (boundErrors == 3)
        {
            Message += "Межі інтегралу введено не правильно!";
            MessageColor = "#eb4034";
        }
        else if (boundErrors > 0)
        {
            Message += "Деякі методи не змогли зчитати межі інтегралу!";
            MessageColor = "#cfa80e";
        }
        else if (nans == 3)
        {
            Message += "Інтеграл не можливо розв'язати нашими методами!";
            MessageColor = "#eb4034";
        }
        else if (nans > 0)
        {
            Message += "Не всі методи мають розв'язок!";
            MessageColor = "#cfa80e";
        }
        else
        {
            Message = "Успішно!";
            MessageColor = "#00bd10";
        }

        Difficulties = difficulty;
        Results = checkResult;
    }

    string ConvertFunctionMath(string func)
    {
        foreach (var item in Dictionary.Operations)
        {
            PcreRegex regex = new PcreRegex(item.Key);
            while (regex.IsMatch(func))
            {
                func = PcreRegex.Replace(func, item.Key, item.Value);
                Console.WriteLine(func);
            }
        }

        return func;
    }
    
    string ConvertFunctionMarkdown(string func)
    {
        foreach (var item in Dictionary.Markdowns)
        {
            PcreRegex regex = new PcreRegex(item.Key);
            while (regex.IsMatch(func))
            {
                func = PcreRegex.Replace(func, item.Key, item.Value);
                Console.WriteLine(func);
            }
        }

        return func;
    }
}