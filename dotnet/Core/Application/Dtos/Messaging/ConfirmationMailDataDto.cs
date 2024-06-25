using Newtonsoft.Json;

namespace DemoLibrary.Application.Dtos.Messaging;

public class ConfirmationEmailDataDto
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("confirmation_token")]
    public string ConfirmationToken { get; set; }
}