using System.Diagnostics.Metrics;

namespace DemoLibrary.Application.Dtos.People;

public class CreatePersonDto
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Instrument? Instrument { get; set; }
    public string? CellphoneNumber { get; set; }
    public string Email { get; set; }
    public string? CityName { get; set; }
    public string ConfirmationToken { get; set; }
}