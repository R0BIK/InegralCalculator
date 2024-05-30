namespace IntegralCalculator;

public class FunctionException : Exception
{
    public FunctionException() { }
    
    public FunctionException(string? message)
        : base(message) { }
    
    public FunctionException(string? message, Exception? innerException)
        : base(message, innerException) { }
}

public class BoundException : Exception
{
    public BoundException() { }
    
    public BoundException(string? message)
        : base(message) { }
    
    public BoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
}

public class NanException : Exception
{
    public NanException() { }
    
    public NanException(string? message)
        : base(message) { }
    
    public NanException(string? message, Exception? innerException)
        : base(message, innerException) { }
}