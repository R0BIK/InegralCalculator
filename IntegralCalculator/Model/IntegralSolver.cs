using NCalc;

namespace IntegralCalculator.Model;

public static class IntegralSolver
{
    public static ulong Difficulty = 0;

    public static int Precision = 1000;
    
    public static double SolveByRectangles(Expression func, double upperBound, double lowerBound, string argument)
    {
        Difficulty = 0;
        double temp = lowerBound;
        double result = 0;
        
        double h = (upperBound - lowerBound) / Precision;

        try
        {
            for (int i = 0; i < Precision; ++i)
            {
                double arg = temp + h / 2;
                func.Parameters[argument] = arg;
                result += Convert.ToDouble(func.Evaluate());
                temp += h;
                Difficulty++;
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
        Difficulty = 0;
        double temp = lowerBound;
        double paired = 0;
        double unpaired = 0;
        double result;
        
        double h = (upperBound - lowerBound) / Precision;
        
        try
        {
            func.Parameters[argument] = lowerBound;
            result = Convert.ToDouble(func.Evaluate());
            func.Parameters[argument] = upperBound;
            result += Convert.ToDouble(func.Evaluate());
            
            for (int i = 1; i < Precision; ++i)
            {
                temp += h;
                func.Parameters[argument] = temp;
                if (i % 2 == 0)
                    paired += Convert.ToDouble(func.Evaluate());
                else
                {
                    unpaired += Convert.ToDouble(func.Evaluate());
                }
                Difficulty++;
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
        Difficulty = 0;
        double temp = lowerBound;
        double result;

        double h = (upperBound - lowerBound) / Precision;

        try
        {
            func.Parameters[argument] = lowerBound;
            result = Convert.ToDouble(func.Evaluate());
            func.Parameters[argument] = upperBound;
            result = (result + Convert.ToDouble(func.Evaluate())) / 2;
            
            for (int i = 1; i < Precision; ++i)
            { 
                temp += h;
                func.Parameters[argument] = temp;
                result += Convert.ToDouble(func.Evaluate());
                Difficulty++;
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