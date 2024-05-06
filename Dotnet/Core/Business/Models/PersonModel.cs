using DemoLibrary.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoLibrary.Models;

public enum Instrument
{
    Guitar,
    Piano,
    Drums,
    Violin,
    Bass,
    Singer,
    Saxophone
}

[Index(nameof(Id), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class PersonModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Instrument Instrument { get; set; }
    public string CellphoneNumber { get; set; }
    public string Email { get; set; }
    public string? CityName { get; set; }
    public bool Verified { get; set; } = false;
    public virtual ICollection<PictureModel> Pictures { get; set; }
}