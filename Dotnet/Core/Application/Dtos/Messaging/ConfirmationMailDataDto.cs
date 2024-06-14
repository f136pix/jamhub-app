using Newtonsoft.Json;

namespace DemoLibrary.Application.Dtos.Messaging;

public class ConfirmationEmailDataDto
{
    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("confirmation_token")]
    public string ConfirmationToken { get; set; }
}