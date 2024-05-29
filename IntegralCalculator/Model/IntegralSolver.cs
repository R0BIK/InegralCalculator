using System.Text.RegularExpressions;
using NCalc;

namespace IntegralCalculator.Model;

public class IntegralSolver
{
    public static ulong Dificulty;
    public static string SolveByRectangles(Expression func, Expression upperB, Expression lowerB, string argument)
    {
        double upperBound;
        double lowerBound;
        try
        {
            upperBound = Convert.ToDouble(upperB.Evaluate());
            lowerBound = Convert.ToDouble(lowerB.Evaluate());
        }
        catch (Exception e)
        {
            return "BoundError";
        }
        
        int precision = 10;
        double temp = lowerBound;
        double result = 0;
        List<double> arguments = new List<double>();
        
        double h = (upperBound - lowerBound) / precision;

        try
        {
            for (int i = 0; i < precision; ++i)
            {
                Dificulty++;
                double arg = temp + h / 2;
                func.Parameters[argument] = arg;
                arguments.Add(Convert.ToDouble(func.Evaluate()));
                temp += h;
            }
            
            for (int i = 0; i < precision; i++)
            {
                Dificulty++;
                result += arguments[i];
            }

            result *= h;
        }
        catch (Exception e)
        {
            return "MathError";
        }

        if (result is Double.NaN)
            return "NaN";
        
        return Math.Round(result, 10).ToString();
    }
    
    public static string SolveBySipmpson(Expression func, Expression upperB, Expression lowerB, string argument)
    {
        double upperBound;
        double lowerBound;
        try
        {
            upperBound = Convert.ToDouble(upperB.Evaluate());
            lowerBound = Convert.ToDouble(lowerB.Evaluate());
        }
        catch (Exception e)
        {
            return "BoundError";
        }
        int precision = 10;
        double temp = lowerBound;
        double paired = 0;
        double unpaired = 0;
        double result = 0;
        List<double> arguments = new List<double>();
        
        double h = (upperBound - lowerBound) / precision;
        
        try
        {
            for (int i = 0; i <= precision; ++i)
            {
                Dificulty++;
                func.Parameters[argument] = temp;
                arguments.Add(Convert.ToDouble(func.Evaluate()));
                temp += h;
            }
            
            result = arguments[0] + arguments[precision];
        
            for (int i = 1; i < precision; i++)
            {
                Dificulty++;
                if (i % 2 == 0)
                    paired += arguments[i];
                else
                {
                    unpaired += arguments[i];
                }
            }

            result = (h / 3) * (result + 2 * paired + 4 * unpaired);
        }
        catch (Exception e)
        {
            return "MathError";
        }
        
        if (result is Double.NaN)
            return "NaN";
        
        return Math.Round(result, 10).ToString();
    }
    
    public static string SolveByTrapezium(Expression func, Expression upperB, Expression lowerB, string argument)
    {
        double upperBound;
        double lowerBound;
        try
        {
            upperBound = Convert.ToDouble(upperB.Evaluate());
            lowerBound = Convert.ToDouble(lowerB.Evaluate());
        }
        catch (Exception e)
        {
            return "BoundError";
        }
        int precision = 10;
        double temp = lowerBound;
        double result = 0;
        List<double> arguments = new List<double>();

        double h = (upperBound - lowerBound) / precision;

        try
        {
            for (int i = 0; i <= precision; ++i)
            { 
                Dificulty++;
                func.Parameters[argument] = temp;
                arguments.Add(Convert.ToDouble(func.Evaluate()));
                temp += h;
            }
            
            result = (arguments[0] + arguments[precision]) / 2;

            for (int i = 1; i < precision; ++i)
            {
                Dificulty++;
                result += arguments[i];
            }

            result *= h;
        }
        catch (Exception e)
        {
            return "MathError";
        }
        
        if (result is Double.NaN)
            return "NaN";
        
        return Math.Round(result, 10).ToString();
    }
}