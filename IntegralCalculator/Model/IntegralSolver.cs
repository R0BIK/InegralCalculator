using NCalc;

namespace IntegralCalculator.Model;

public static class IntegralSolver
{
    private static ulong Difficulty;

    public static ulong GetDifficulty()
    {
        ulong temp = Difficulty;
        Difficulty = 0;
        return temp;
    }
    public static double SolveByRectangles(Expression func, double upperBound, double lowerBound, string argument)
    {
        int precision = 1000;
        double temp = lowerBound;
        double result = 0;
        List<double> arguments = new List<double>();
        
        double h = (upperBound - lowerBound) / precision;

        try
        {
            for (int i = 0; i < precision; ++i)
            {
                Difficulty++;
                double arg = temp + h / 2;
                func.Parameters[argument] = arg;
                arguments.Add(Convert.ToDouble(func.Evaluate()));
                temp += h;
            }
            
            for (int i = 0; i < precision; i++)
            {
                Difficulty++;
                result += arguments[i];
            }

            result *= h;
        }
        catch (Exception e)
        {
            throw new FunctionException("Cannot convert function", e);
        }

        if (result is Double.NaN)
            throw new NanException("The result is NaN");
        
        return Math.Round(result, 10);
    }
    
    public static double SolveBySipmpson(Expression func, double upperBound, double lowerBound, string argument)
    {
        int precision = 1000;
        double temp = lowerBound;
        double paired = 0;
        double unpaired = 0;
        double result;
        List<double> arguments = new List<double>();
        
        double h = (upperBound - lowerBound) / precision;
        
        try
        {
            for (int i = 0; i <= precision; ++i)
            {
                Difficulty++;
                func.Parameters[argument] = temp;
                arguments.Add(Convert.ToDouble(func.Evaluate()));
                temp += h;
            }
            
            result = arguments[0] + arguments[precision];
        
            for (int i = 1; i < precision; i++)
            {
                Difficulty++;
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
            throw new FunctionException("Cannot convert function", e);
        }
        
        if (result is Double.NaN)
            throw new NanException("The result is NaN");
        
        return Math.Round(result, 10);
    }
    
    public static double SolveByTrapezium(Expression func, double upperBound, double lowerBound, string argument)
    {
        int precision = 1000;
        double temp = lowerBound;
        double result = 0;
        List<double> arguments = new List<double>();

        double h = (upperBound - lowerBound) / precision;

        try
        {
            for (int i = 0; i <= precision; ++i)
            { 
                Difficulty++;
                func.Parameters[argument] = temp;
                arguments.Add(Convert.ToDouble(func.Evaluate()));
                temp += h;
            }
            
            result = (arguments[0] + arguments[precision]) / 2;

            for (int i = 1; i < precision; ++i)
            {
                Difficulty++;
                result += arguments[i];
            }

            result *= h;
        }
        catch (Exception e)
        {
            throw new FunctionException("Cannot convert function", e);
        }
        
        if (result is Double.NaN)
            throw new NanException("The result is NaN");
        
        return Math.Round(result, 10);
    }
}