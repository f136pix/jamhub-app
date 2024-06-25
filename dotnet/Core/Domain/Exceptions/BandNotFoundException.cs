namespace DemoLibrary.Domain.Exceptions;

public class BandNotFoundException : Exception
{
    public BandNotFoundException(int id)
        : base($"Band with id {id} not found.")
    {
    }
}