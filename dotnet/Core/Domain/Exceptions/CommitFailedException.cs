namespace DemoLibrary.Business.Exceptions;

public class CommitFailedException : Exception
{
    public CommitFailedException() : base("Commit operation failed.")
    {
    }
}