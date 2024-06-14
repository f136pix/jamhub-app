namespace DemoLibrary.Domain.Exceptions;

public class CouldNotCreateExchangeException : Exception
{
    public CouldNotCreateExchangeException(string err) : base($"{err.ToString()}")
    {
    }
}