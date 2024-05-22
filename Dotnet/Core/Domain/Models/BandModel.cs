using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoLibrary.Domain.Models;

public enum BandRoles
{
    Creator,
    Participant
}

[Index(nameof(Name), IsUnique = true)]
public class BandModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Genre { get; set; }
    public string CityName { get; set; }
    public int CreatorId { get; set; }
    public PersonModel Creator { get; set; }
    // bands can haveMany people/members
    public virtual ICollection<PersonModel>? Members { get; set; }
}