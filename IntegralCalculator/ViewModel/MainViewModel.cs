using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegralCalculator.Model;
using Microsoft.VisualBasic;
using NCalc;
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
                Message = "Успішно!";
                MessageColor = "#00bd10";
                ShowIntegral = @"\int_" + $"{lowerB}" + @"^{" + $"{upperB}" + "}{" + $"{markdownFunction}" + @"\, dx}";
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
        
        Console.WriteLine(mathFunction);
        
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
            IntegralSolver.SolveBySipmpson(function, upper, lower, SelectedItem.Formula),
            IntegralSolver.SolveByRectangles(function, upper, lower, SelectedItem.Formula),
            IntegralSolver.SolveByTrapezium(function, upper, lower, SelectedItem.Formula)
        };
        
        
        // Console.WriteLine(markdownFunction);
        // Console.WriteLine(@"\int_" + $"{LowerBound}" + @"^{\" + $"{UpperBound}" + "}{" + $"{markdownFunction}" + @"\, dx} =");
    }

    string ConvertFunctionMath(string func)
    {
        foreach (var item in Dictionary.Operations)
        {
            Regex regex = new Regex(item.Key);
            while (regex.IsMatch(func))
            {
                func = Regex.Replace(func, item.Key, item.Value);
            }
        }

        return func;
    }
    
    string ConvertFunctionMarkdown(string func)
    {
        foreach (var item in Dictionary.Markdowns)
        {
            Regex regex = new Regex(item.Key);
            while (regex.IsMatch(func))
            {
                func = Regex.Replace(func, item.Key, item.Value);
            }
        }

        return func;
    }
}