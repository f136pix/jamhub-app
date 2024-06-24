using DemoLibrary.Application.DataAccess;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DemoLibrary.Infraestructure.DataAccess;

public class BlacklistRepository : ITokenRepository<BlacklistedToken>

{
    private readonly ApplicationDbContext _context;

    public BlacklistRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BlacklistedToken> InsertBlacklistAsync(BlacklistedToken blacklistedToken)
    {
        await _context.BlacklistedTokens.AddAsync(blacklistedToken);
        return blacklistedToken;
    }


    public Task<IReadOnlyList<BlacklistedToken>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    // <param name="jti">An <see cref="XmlConfigResource" /> jti from the token who must be retrieved.</param>
    public Task<BlacklistedToken> GetByIdAsync(string jti)
    {
        return _context.BlacklistedTokens.FirstOrDefaultAsync(x => x.Jti == jti);
    }

    public Task<BlacklistedToken> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<BlacklistedToken> AddAsync(BlacklistedToken entity)
    {
        await _context.BlacklistedTokens.AddAsync(entity);
        return entity;
    }

    public Task<BlacklistedToken> UpdateAsync(BlacklistedToken entity)
    {
        throw new NotImplementedException();
    }
}