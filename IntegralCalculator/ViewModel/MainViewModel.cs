using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegralCalculator.Model;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PCRE;
using WpfMath.Controls;
using Expression = NCalc.Expression;
using Regex = IntegralCalculator.Model.Regex;

namespace IntegralCalculator.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private double _upperB;
    private double _lowerB;
    private Expression _mathExpression = new Expression("");
    
    [ObservableProperty]
    private int _graphPoints = 1000;
    
    [ObservableProperty]
    private int _precision = IntegralSolver.Precision;
    
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
    
    public ObservableCollection<string> Results { get; set; }
    
    public ObservableCollection<ulong> Difficulties { get; set; }
    
    public PlotModel? PlotModel1 { get; set; }

    public ObservableCollection<FormulaControl> Options { get; private set; } = null!;
    
    public FormulaControl SelectedItem { get; set; } = null!;
    
    public ICommand ResolveCommand { get; set; }
    
    public ICommand SaveFileCommand { get; set; }
    
    public MainViewModel()
    {
        Difficulties = new ObservableCollection<ulong>();
        Results = ["", "", ""];
        InitializeGraph();
        InitComboBox();
        ResolveCommand = new RelayCommand(CheckInput);
        SaveFileCommand = new RelayCommand(SaveFile);
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
            IntegralSolver.Precision = Precision;
            ConvertInputInMath();
            string markdownFunction = ConvertFunctionMarkdown(Function);
            string upperB = ConvertFunctionMarkdown(UpperBound);
            string lowerB = ConvertFunctionMarkdown(LowerBound);
            ShowIntegral = @"\int_{" + $"{lowerB}" + @"}^{" + $"{upperB}" + "}{" + $"{markdownFunction}" + @"\, d" + $"{SelectedItem.Formula}" + "}";
        }
    }

    private void ConvertInputInMath()
    {
        Difficulties.Clear();
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
            for (int i = 0; i < 3; ++i)
            {
                Results[i] = "Cannot convert bounds";
                Difficulties.Add(0);
            }
            ValidateResult();
            return;
        }
        
        CallSolvers();
    }

    private void CallSolvers()
    {
        try
        {
            Results[0] = IntegralSolver.SolveBySipmpson(_mathExpression, _upperB, _lowerB, SelectedItem.Formula).ToString();
        }
        catch (Exception e)
        {
            Results[0] = e.Message;
        }
        Difficulties.Add(IntegralSolver.Difficulty);
        try
        {
            Results[1] = IntegralSolver.SolveByRectangles(_mathExpression, _upperB, _lowerB, SelectedItem.Formula).ToString();
        }
        catch (Exception e)
        {
            Results[1] = e.Message;
        }
        Difficulties.Add(IntegralSolver.Difficulty);
        try
        {
            Results[2] = IntegralSolver.SolveByTrapezium(_mathExpression, _upperB, _lowerB, SelectedItem.Formula).ToString();
        }
        catch (Exception e)
        {
            Results[2] = e.Message;
        }
        Difficulties.Add(IntegralSolver.Difficulty);
        
        ValidateResult();
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
            Message = "Функцію введено не правильно! ";
            MessageColor = "#eb4034";
        }
        else if (mathErrors > 0)
        {
            Message += "Деякі методи не змогли зчитати функцію! ";
            MessageColor = "#cfa80e";
        }
        else if (boundsErrors > 0)
        {
            Message = "Межі інтегралу введено не правильно! ";
            MessageColor = "#eb4034";
        }
        else if (nans == 3)
        {
            Message = "Інтеграл не можливо розв'язати нашими методами! ";
            MessageColor = "#eb4034";
        }
        else if (nans > 0)
        {
            Message += "Не всі методи мають розв'язок! ";
            MessageColor = "#cfa80e";
        }
        else
        {
            Message = "Успішно! ";
            MessageColor = "#00bd10";
        }

        try
        {
            DrawGraph();
        }
        catch (Exception e)
        {
            Message += "Помилка в побудові графіку ";
            MessageColor = "#cfa80e";
        }
    }

    private string ConvertFunctionMath(string func)
    {
        foreach (var item in Regex.Operations)
        {
            PcreRegex regex = new PcreRegex(item.Key);
            while (regex.IsMatch(func))
            {
                func = PcreRegex.Replace(func, item.Key, item.Value);
            }
        }

        return func;
    }
    
    private string ConvertFunctionMarkdown(string func)
    {
        foreach (var item in Regex.Markdowns)
        {
            PcreRegex regex = new PcreRegex(item.Key);
            while (regex.IsMatch(func))
            {
                func = PcreRegex.Replace(func, item.Key, item.Value);
            }
        }

        return func;
    }

    private bool IsNumber(string text)
    {
        return Char.IsDigit(text[^1]);
    }
    
    private void InitializeGraph()
    {
        PlotModel1 = new PlotModel
        {
            Title = "Графік інтегралу",
            Background = OxyColors.White
        };

        var xAxis = new LinearAxis
        {
            Position = AxisPosition.Bottom,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Black,
            PositionAtZeroCrossing = true,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot,
            MajorGridlineColor = OxyColors.LightGray,
            MinorGridlineColor = OxyColors.LightGray,
            Title = "Вісь Х",
        };

        var yAxis = new LinearAxis
        {
            Position = AxisPosition.Left,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Black,
            PositionAtZeroCrossing = true,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot,
            MajorGridlineColor = OxyColors.LightGray,
            MinorGridlineColor = OxyColors.LightGray,
            Title = "Вісь Y",
        };
        
        PlotModel1.Axes.Add(xAxis);
        PlotModel1.Axes.Add(yAxis);
    }
    
    private void DrawGraph()
    {
        InitializeGraph();

        foreach (var t in Results)
        {
            if (IsNumber(t))
            {
                var lineSeries = new AreaSeries
                {
                    Color = OxyColors.Coral,
                    MarkerType = MarkerType.None, 
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Red,
                    MarkerFill = OxyColors.Red
                };
        
                List<double> xValues = new List<double>();
                List<double> yValues = new List<double>();
                double step = (_upperB - _lowerB) / GraphPoints;
                for (int i = 0; i <= GraphPoints; ++i)
                {
                    double x = _lowerB + step * i;
                    if (double.IsPositiveInfinity(x))
                    {
                        x = Int32.MaxValue;
                        Message += "В графіку змодельована нескінченість ";
                        MessageColor = "#cfa80e";
                    }
                    else if (double.IsNegativeInfinity(x))
                    {
                        x = Int32.MinValue;
                        Message += "В графіку змодельована нескінченість ";
                        MessageColor = "#cfa80e";
                    }
                    xValues.Add(x);
                    _mathExpression.Parameters[SelectedItem.Formula] = xValues[i]; 
                    double y = Convert.ToDouble(_mathExpression.Evaluate());
                    if (double.IsPositiveInfinity(y))
                    {
                        y = Int32.MaxValue;
                        Message += "В графіку змодельована нескінченість ";
                        MessageColor = "#cfa80e";
                    }
                    else if (double.IsNegativeInfinity(y))
                    {
                        y = Int32.MinValue;
                        Message += "В графіку змодельована нескінченість ";
                        MessageColor = "#cfa80e";
                    }
                    yValues.Add(y);
                    
                    lineSeries.Points.Add(new DataPoint(xValues[i], yValues[i]));
                    lineSeries.Points2.Add(new DataPoint(xValues[i], 0));
                }
                
                PlotModel1.Series.Add(lineSeries);
            }
        }
    }

    private void SaveFile()
    {
        try
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Calculator_Log.txt");
            
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine("Input func:\n" + Function);
                sw.WriteLine("Input upper bound:\n" + UpperBound);
                sw.WriteLine("Input lower bound:\n" + LowerBound);
                sw.WriteLine("Parsed Math func:\n" + ConvertFunctionMath(Function));
                sw.WriteLine("Parsed Math upper bound:\n" + ConvertFunctionMath(UpperBound));
                sw.WriteLine("Parsed Math lower bound:\n" + ConvertFunctionMath(LowerBound));
                sw.WriteLine("Parsed Markdown integral:\n" + @"\int_{" + $"{ConvertFunctionMarkdown(LowerBound)}" + @"}^{" + $"{ConvertFunctionMarkdown(UpperBound)}" + "}{" + $"{ConvertFunctionMarkdown(Function)}" + @"\, d" + $"{SelectedItem.Formula}" + "}");
                sw.WriteLine("Simpson method:\n" + Results[0]);
                sw.WriteLine("Rectangles method:\n" + Results[1]);
                sw.WriteLine("Trapezium method:\n" + Results[2]);
            }
        }
        catch (Exception e)
        {
            Message = "Не вдалось зберегти файл!";
            MessageColor = "#eb4034";
            return;
        }
        
        Message = "Файл успішно збережено на ваш робочий стіл!";
        MessageColor = "#00bd10";
    }
}