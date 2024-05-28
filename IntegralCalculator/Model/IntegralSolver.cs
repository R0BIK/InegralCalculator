using System.Text.RegularExpressions;
using NCalc;

namespace IntegralCalculator.Model;

public class IntegralSolver
{
    public static string SolveByRectangles(Expression func, Expression upperB, Expression lowerB, string argument)
    {
        double upperBound = Convert.ToDouble(upperB.Evaluate());
        double lowerBound = Convert.ToDouble(lowerB.Evaluate());
        int precision = 10;
        double temp = lowerBound;
        double result = 0;
        List<double> arguments = new List<double>();
        
        double h = (upperBound - lowerBound) / precision;

        try
        {
            for (int i = 0; i < precision; ++i)
            {
                double arg = temp + h / 2;
                func.Parameters[argument] = arg;
                arguments.Add(Convert.ToDouble(func.Evaluate()));
                temp += h;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        for (int i = 0; i < precision; i++)
        {
            result += arguments[i];
        }

        result *= h;
        Console.WriteLine(result);
        return result.ToString();
    }
    
    public static string SolveBySipmpson(Expression func, Expression upperB, Expression lowerB, string argument)
    {
        double upperBound = Convert.ToDouble(upperB.Evaluate());
        double lowerBound = Convert.ToDouble(lowerB.Evaluate());
        int precision = 10;
        double temp = lowerBound;
        double paired = 0;
        double unpaired = 0;
        List<double> arguments = new List<double>();
        
        double h = (upperBound - lowerBound) / precision;
        
        try
        {
            for (int i = 0; i <= precision; ++i)
            {
                func.Parameters[argument] = temp;
                arguments.Add(Convert.ToDouble(func.Evaluate()));
                temp += h;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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
        return result.ToString();
    }
    
    public static string SolveByTrapezium(Expression func, Expression upperB, Expression lowerB, string argument)
    {
        double upperBound = Convert.ToDouble(upperB.Evaluate());
        double lowerBound = Convert.ToDouble(lowerB.Evaluate());
        int precision = 10;
        double temp = lowerBound;
        List<double> arguments = new List<double>();

        double h = (upperBound - lowerBound) / precision;

        try
        {
            for (int i = 0; i <= precision; ++i)
            { 
                func.Parameters[argument] = temp;
                arguments.Add(Convert.ToDouble(func.Evaluate()));
                temp += h;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        double result = (arguments[0] + arguments[precision]) / 2;

        for (int i = 1; i < precision; ++i)
        {
            result += arguments[i];
        }

        result *= h;
        
        Console.WriteLine(result);
        return result.ToString();
    }
}