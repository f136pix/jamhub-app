namespace DemoLibrary.Domain.Exceptions;

public class CouldNotValidateUser : Exception
{
    public CouldNotValidateUser(string err) : base($"{err.ToString()}")
    {
    }
}