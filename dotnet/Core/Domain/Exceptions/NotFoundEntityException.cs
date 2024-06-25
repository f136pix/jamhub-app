namespace DemoLibrary.Domain.Exceptions;

public class NotFoundEntityException : Exception
{
    public NotFoundEntityException(string entity, string id)
        : base($"{entity} with id {id} not found.i")
    {
    }
}

