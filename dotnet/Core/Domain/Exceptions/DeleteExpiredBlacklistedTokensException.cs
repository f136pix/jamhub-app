using DemoLibrary.Domain.Models;

namespace DemoLibrary.Domain.Exceptions;

public class DeleteExpiredBlacklistedTokensException : Exception
{
    public List<BlacklistedToken> deletedTokens;
    public List<BlacklistedToken> errorTokens;

    public DeleteExpiredBlacklistedTokensException(List<BlacklistedToken> deletedTokens,
        List<BlacklistedToken> errorTokens) : base($"There was a error deleting one or more token")
    {
        deletedTokens = deletedTokens;
        errorTokens = errorTokens;
    }
}

