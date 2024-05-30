using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IntegralCalculator.ViewModel;

namespace IntegralCalculator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ShowGraph(object sender, RoutedEventArgs e)
    {
        Window graph = new GraphWindow()
        {
            ShowInTaskbar = false,
            Owner = this,
            DataContext = this.DataContext,
        };
        
        graph.Show();
    }
    
    private void ShowInfo(object sender, RoutedEventArgs e)
    {
        Window info = new InfoWindow()
        {
            ShowInTaskbar = false,
            Owner = this,
        };
        
        info.Show();
    }
}