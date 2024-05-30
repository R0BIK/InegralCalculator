using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegralCalculator.Model;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using PCRE;
using WpfMath.Controls;
using Expression = NCalc.Expression;

namespace IntegralCalculator.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private double _upperB = 0;
    private double _lowerB = 0;
    private Expression _mathExpression = new Expression("");
    
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

    // [ObservableProperty]
    // private List<string> _results;
    
    public ObservableCollection<string> Results { get; set; }
    
    public ObservableCollection<ulong> Difficulties { get; set; }
    
    public PlotModel PlotModel1 { get; set; }

    public ObservableCollection<FormulaControl> Options { get; private set; } = null!;
    
    public FormulaControl SelectedItem { get; set; } = null!;
    
    public ICommand ResolveCommand { get; set; }
    
    public MainViewModel()
    {
        Results = ["", "", ""];
        Difficulties = new ObservableCollection<ulong>();
        InitComboBox();
        ResolveCommand = new RelayCommand(CheckInput);
    }

    private void InitComboBox()
    {
        var availableArguments = new List<string>
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
            FormulaControl formulaControl = new FormulaControl
            {
                Formula = availableArguments[i],
                Scale = 26
            };
            Options.Add(formulaControl);
        }
        
        SelectedItem = Options[0];
    }

    private void CheckInput()
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
            ShowIntegral = @"\int" + "{" + $"{markdownFunction}" + @"\, d" + $"{SelectedItem.Formula}" + "}";
        }
        else if (LowerBound == "")
        {
            Message = "Введіть нижню межу інтегралу в текстовому полі!";
            MessageColor = "#eb4034";
            string markdownFunction = ConvertFunctionMarkdown(Function);
            ShowIntegral = @"\int" + "{" + $"{markdownFunction}" + @"\, d" + $"{SelectedItem.Formula}" + "}";
        }
        else
        {
            ConvertInputInMath();
            string markdownFunction = ConvertFunctionMarkdown(Function);
            string upperB = ConvertFunctionMarkdown(UpperBound);
            string lowerB = ConvertFunctionMarkdown(LowerBound);
            ShowIntegral = @"\int_{" + $"{lowerB}" + @"}^{" + $"{upperB}" + "}{" + $"{markdownFunction}" + @"\, d" + $"{SelectedItem.Formula}" + "}";
            Console.WriteLine(markdownFunction);
            Console.WriteLine(@"\int_{" + $"{lowerB}" + @"}^{" + $"{upperB}" + "}{" + $"{markdownFunction}" + @"\, d" + $"{SelectedItem.Formula}" + "}");
        }
    }

    private void ConvertInputInMath()
    {
        string mathFunction = ConvertFunctionMath(Function);
        string upperB = ConvertFunctionMath(UpperBound);
        string lowerB = ConvertFunctionMath(LowerBound);
        
        _mathExpression = new Expression(mathFunction)
        {
            Parameters =
            {
                ["PI"] = Math.PI,
                ["E"] = Math.E
            }
        };

        Expression upper = new Expression(upperB)
        {
            Parameters =
            {
                ["PI"] = Math.PI,
                ["E"] = Math.E
            }
        };

        Expression lower = new Expression(lowerB)
        {
            Parameters =
            {
                ["PI"] = Math.PI,
                ["E"] = Math.E
            }
        };
        
        try
        {
            _upperB = Convert.ToDouble(upper.Evaluate());
            _lowerB = Convert.ToDouble(lower.Evaluate());
        }
        catch (Exception e)
        {
            Results[0] = "Cannot convert bounds";
            Results[1] = "Cannot convert bounds";
            Results[2] = "Cannot convert bounds";
            ValidateResult();
            return;
        }
        
        CallSolvers();
    }

    private void CallSolvers()
    {
        Difficulties.Clear();   

        try
        {
            Results[0] = IntegralSolver.SolveBySipmpson(_mathExpression, _upperB, _lowerB, SelectedItem.Formula).ToString();
        }
        catch (Exception e)
        {
            Results[0] = e.Message;
        }
        Difficulties.Add(IntegralSolver.GetDifficulty());
        try
        {
            Results[1] = IntegralSolver.SolveByRectangles(_mathExpression, _upperB, _lowerB, SelectedItem.Formula).ToString();
        }
        catch (Exception e)
        {
            Results[1] = e.Message;
        }
        Difficulties.Add(IntegralSolver.GetDifficulty());
        try
        {
            Results[2] = IntegralSolver.SolveByTrapezium(_mathExpression, _upperB, _lowerB, SelectedItem.Formula).ToString();
        }
        catch (Exception e)
        {
            Results[2] = e.Message;
        }
        Difficulties.Add(IntegralSolver.GetDifficulty());
        
        ValidateResult();
        // DrawGraphic(function, upperBound, lowerBound);
    }

    private void ValidateResult()
    {
        int mathErrors = 0;
        int boundsErrors = 0;
        int nans = 0;
        Message = "";

        for (int i = 0; i < Results.Count; ++i)
        {
            if (Results[i] == "Cannot convert function")
            {
                mathErrors++;
                Results[i] = "-";
            }
            else if (Results[i] == "Cannot convert bounds")
            {
                boundsErrors++;
                Results[i] = "-";
            }
            else if (Results[i] == "The result is NaN")
            {
                Results[i] = "Не має натуральних розв'язків";
                nans++;
            }
        }

        if (mathErrors == 3)
        {
            Message = "Функцію введено не правильно!";
            MessageColor = "#eb4034";
        }
        else if (mathErrors > 0)
        {
            Message += "Деякі методи не змогли зчитати функцію!";
            MessageColor = "#cfa80e";
        }
        else if (boundsErrors > 0)
        {
            Message = "Межі інтегралу введено не правильно!";
            MessageColor = "#eb4034";
        }
        else if (nans == 3)
        {
            Message = "Інтеграл не можливо розв'язати нашими методами!";
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
    }

    private string ConvertFunctionMath(string func)
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
    
    private string ConvertFunctionMarkdown(string func)
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

    void DrawGraphic(Expression functiion, double upperBound, double lowerBound)
    {
        PlotModel1 = new PlotModel { Title = "Графік" };
        PlotModel1.Background = OxyColors.White;
        var xAxis = new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X",
            Minimum = 0,
            Maximum = 1
        };

        var yAxis = new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Интеграл f(x)",
            Minimum = 0,
            Maximum = 0.5
        };

        // Добавляем оси в модель
        PlotModel1.Axes.Add(xAxis);
        PlotModel1.Axes.Add(yAxis);
        
        var lineSeries = new LineSeries
        {
            Title = "Интеграл",
            MarkerType = MarkerType.Circle, // Добавим маркеры для визуализации точек
            MarkerSize = 3,
            Color = OxyColors.Blue,
            MarkerStroke = OxyColors.Red,
            MarkerFill = OxyColors.Red
        };

        List<double> xValues = new List<double>();
        List<double> yValues = new List<double>();
        int n = 100;
        double step = (upperBound - lowerBound) / n;

        for (int i = 0; i < n; ++i)
        {
            xValues.Add(lowerBound + step * i);
            yValues.Add(IntegralSolver.SolveBySipmpson(functiion, xValues[i], lowerBound, SelectedItem.Formula));
            lineSeries.Points.Add(new DataPoint(xValues[i], yValues[i]));
        }
        
        PlotModel1.Series.Add(lineSeries);
    }
}