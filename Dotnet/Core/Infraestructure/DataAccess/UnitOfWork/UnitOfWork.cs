using DemoLibrary.Business.Exceptions;
using DemoLibrary.Infraestructure.DataAccess.Context;

namespace DemoLibrary.Infraestructure.DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw new CommitFailedException();
        }
    }
}