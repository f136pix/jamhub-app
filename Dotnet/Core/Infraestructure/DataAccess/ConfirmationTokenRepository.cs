using DemoLibrary.Application.DataAccess;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DemoLibrary.Infraestructure.DataAccess;

public interface IConfirmationTokenRepository
{
    Task<ConfirmationToken> InsertConfirmationTokenAsync(ConfirmationToken confirmationToken);
    Task<ConfirmationToken> GetConfirmationTokenByToken(string token);

    // Task<bool> ConfirmUserByTokenAsync(string token);
}

public class ConfirmationTokenRepository : ITokenRepository<ConfirmationToken>
{
    private readonly ApplicationDbContext _context;

    public ConfirmationTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public Task<IReadOnlyList<ConfirmationToken>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ConfirmationToken> AddAsync(ConfirmationToken confirmationToken)
    {
        await _context.ConfirmationTokens.AddAsync(confirmationToken);
        return confirmationToken;
    }

    public async Task<ConfirmationToken> UpdateAsync(ConfirmationToken entity)
    {
        var token = await _context.ConfirmationTokens.FindAsync(entity.Token);
        token = entity;

        return entity;
    }

    // <param name="token">An <see cref="XmlConfigResource" /> token that must be retrieved.</param>

    public async Task<ConfirmationToken> GetByIdAsync(string token)
    {
        return await _context.ConfirmationTokens
            .FirstOrDefaultAsync(c => c.Token == token);
    }

    public Task<ConfirmationToken> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    // method has too much business logic and should be in a service
    // public async Task<bool> ConfirmUserByTokenAsync(string token)
    // {
    //     var confirmationToken = await _context.ConfirmationToken
    //         .Include(c => c.User)
    //         .FirstOrDefaultAsync(c => c.Token == token);
    //
    //     if (confirmationToken == null)
    //         throw new CouldNotValidateUser("Invalid token");
    //
    //     // if user is already validated
    //     if (confirmationToken.User.Verified == true)
    //     {
    //         throw new CouldNotValidateUser("User already verified");
    //     }
    //
    //     confirmationToken.User.Verified = true;
    //     return true;
    // }
}