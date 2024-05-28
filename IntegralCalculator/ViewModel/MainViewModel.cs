using System.Text.RegularExpressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegralCalculator.Model;

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
    
    public ICommand ResolveCommand { get; set; }
    
    public MainViewModel()
    {
        IntegralSolver.SolveByTrapezium();
        IntegralSolver.SolveBySipmpson();
        IntegralSolver.SolveByRectangles();
        ShowIntegral = @"\int";
        // Console.WriteLine(@"\int_0^{\0}{x\, dx} =");
        // Console.WriteLine(@"\int_" + "0" + @"^{\" + "0" + @"}{" + "x" + @"\, dx} =");
        // Console.WriteLine(@"\int_" + $"{_lowerBound}" + @"^{" + $"{_upperBound}" + "}{" + $"{_function}" + @"\, dx} =");
        ResolveCommand = new RelayCommand(() =>
        {
            string mathfunction = _function;
            string markdownFunction = _function;
            foreach (var item in Dictionary.operations)
            {
                Regex regex = new Regex(item.Key);
                while (regex.IsMatch(mathfunction))
                {
                    mathfunction = Regex.Replace(mathfunction, item.Key, item.Value);
                }
            }
            foreach (var item in Dictionary.markdowns)
            {
                Regex regex = new Regex(item.Key);
                while (regex.IsMatch(markdownFunction))
                {
                    markdownFunction = Regex.Replace(markdownFunction, item.Key, item.Value);
                }
            }
            Console.WriteLine(mathfunction);
            Console.WriteLine(markdownFunction);
            Console.WriteLine(@"\int_" + $"{_lowerBound}" + @"^{\" + $"{_upperBound}" + "}{" + $"{markdownFunction}" + @"\, dx} =");
            ShowIntegral = @"\int_" + $"{_lowerBound}" + @"^{" + $"{_upperBound}" + "}{" + $"{markdownFunction}" + @"\, dx} =";
        });
    }
}