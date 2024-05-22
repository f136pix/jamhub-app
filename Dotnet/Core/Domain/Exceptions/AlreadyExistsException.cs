namespace DemoLibrary.Business.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string property) : base($"{property} already exists.")
    {
    }
}