using System.Data.SqlTypes;
using DemoLibrary.Application.DataAccess;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.Infraestructure.DataAccess.Context;

namespace DemoLibrary.Infraestructure.DataAccess.UnitOfWork;

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
            Console.WriteLine("--> Unit of work committed successfully");
            return true;
        }


        catch (Exception ex)
        {
            throw new CommitFailedException();
        }
    }
}