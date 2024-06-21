namespace DemoLibrary.Application.DataAccess;
public interface IUnitOfWork
{
    Task<bool> CommitAsync();
}

