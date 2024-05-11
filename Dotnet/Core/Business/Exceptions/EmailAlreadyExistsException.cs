namespace DemoLibrary.Business.Exceptions;

public class EmailAlreadyExistsException : Exception
{
    public EmailAlreadyExistsException() : base("Email already exists.")
    {
    }
}