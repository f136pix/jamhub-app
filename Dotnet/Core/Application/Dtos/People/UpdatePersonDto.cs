using System.Diagnostics.Metrics;
using DemoLibrary.Domain.Models;
using DemoLibrary.Models;

namespace DemoLibrary.Application.Dtos.People;

public class UpdatePersonDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public MusicalInstrument? Instrument { get; set; }
    public string? CellphoneNumber { get; set; }
    // public string Email { get; set; } email should be immutable
    public string? CityName { get; set; }
    public IEnumerable<int>? BandIds { get; set; }
}