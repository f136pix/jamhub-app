using System.ComponentModel.DataAnnotations;
using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoLibrary.Domain.Models;

[Index(nameof(Token), IsUnique = true)]
public class ConfirmationToken
{
    [Key]
    public string Token { get; set; }
    public int UserId { get; set; }
    public virtual Person User { get; set; }
}