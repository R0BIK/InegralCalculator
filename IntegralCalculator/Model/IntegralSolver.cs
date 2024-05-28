using System.Text.RegularExpressions;

namespace IntegralCalculator.Model;

public class IntegralSolver
{
    public static void SolveByRectangles()
    {
        double upperBound = Math.PI/4;
        double lowerBound = 0;
        int precision = 4;
        double temp = lowerBound;
        double result = 0;
        List<double> arguments = new List<double>();
        
        double h = (upperBound - lowerBound) / precision;

        for (int i = 0; i < precision; ++i)
        {
            double arg = temp + h / 2;
            arguments.Add(Math.Sin(arg)/(Math.Pow(arg, 2) - 1));
            temp += h;
        }
        
        for (int i = 0; i < precision; i++)
        {
            result += arguments[i];
        }

        result *= h;
        Console.WriteLine(result);
    }
    
    public static void SolveBySipmpson()
    {
        double upperBound = 2;
        double lowerBound = 1.2;
        int precision = 8;
        double temp = lowerBound;
        double paired = 0;
        double unpaired = 0;
        List<double> arguments = new List<double>();
        
        double h = (upperBound - lowerBound) / precision;

        for (int i = 0; i <= precision; ++i)
        {
            arguments.Add(double.Sqrt(1 + 2 * double.Pow(temp, 2) - double.Pow(temp, 3)));
            temp += h;
        }

        double result = arguments[0] + arguments[precision];
        
        for (int i = 1; i < precision; i++)
        {
            if (i % 2 == 0)
                paired += arguments[i];
            else
            {
                unpaired += arguments[i];
            }
        }

        result = (h / 3) * (result + 2 * paired + 4 * unpaired);
        Console.WriteLine(result);
    }
    
    public static void SolveByTrapezium()
    {
        double upperBound = 5;
        double lowerBound = 2;
        int precision = 6;
        double temp = lowerBound;
        List<double> arguments = new List<double>();

        double h = (upperBound - lowerBound) / precision;

        for (int i = 0; i <= precision; ++i)
        { 
            arguments.Add(1/Math.Log(temp));
            temp += h;
        }

        double result = (arguments[0] + arguments[precision]) / 2;

        for (int i = 1; i < precision; ++i)
        {
            result += arguments[i];
        }

        result *= h;
        
        Console.WriteLine(result);
    }
}