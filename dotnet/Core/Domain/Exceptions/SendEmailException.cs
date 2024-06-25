namespace DemoLibrary.Domain.Exceptions;

public class SendEmailException : Exception
{
    public SendEmailException(string err) : base($"There was a error sending the email : {err.ToString()}")
    {
    }
}