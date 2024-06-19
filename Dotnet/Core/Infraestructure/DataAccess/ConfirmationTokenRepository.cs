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

public class ConfirmationTokenRepository : IConfirmationTokenRepository
{
    private readonly ApplicationDbContext _context;

    public ConfirmationTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<ConfirmationToken> InsertConfirmationTokenAsync(ConfirmationToken confirmationToken)
    {
        await _context.ConfirmationToken.AddAsync(confirmationToken);
        return confirmationToken;
    }

    public async Task<ConfirmationToken> GetConfirmationTokenByToken(string token)
    {
        return await _context.ConfirmationToken
            .FirstOrDefaultAsync(c => c.Token == token);
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