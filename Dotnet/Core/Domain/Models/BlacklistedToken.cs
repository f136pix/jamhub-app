using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DemoLibrary.Domain.Models;

[Index(nameof(Jti), IsUnique = true)]
public class BlacklistedToken
{
    [Key] public string Jti { get; set; }
    public DateTime ExpiryDate { get; set; }
}