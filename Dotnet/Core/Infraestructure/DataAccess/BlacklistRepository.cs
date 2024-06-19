using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DemoLibrary.Infraestructure.DataAccess;

public interface IBlacklistRepository
{
    Task<Blacklist> GetBlacklistByJtiAsync(string jti);
    Task<Blacklist> InsertBlacklistAsync(Blacklist blacklist);
}

public class BlacklistRepository : IBlacklistRepository
{
    private readonly ApplicationDbContext _context;

    public BlacklistRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public Task<Blacklist> GetBlacklistByJtiAsync(string jti)
    {
        return  _context.Blacklist.FirstOrDefaultAsync(x => x.Jti == jti);
    }

    public async Task<Blacklist> InsertBlacklistAsync(Blacklist blacklist)
    {
        await _context.Blacklist.AddAsync(blacklist);
        return blacklist;
    }
    
    
}