namespace DemoLibrary.Application.Dtos.Band;

public class CreateBandDto
{
    public string Name { get; set; }
    public string? Genre { get; set; }
    public string CityName { get; set; }
    public int CreatorId { get; set; }
}