using Newtonsoft.Json;

namespace DemoLibrary.Application.Dtos.Blacklist;

public class CreateBlacklistedTokenDto
{
    [JsonProperty("jti")]
    public string Jti { get; set; }
    
    [JsonProperty("exp")]
    public DateTime ExpiryDate { get; set; }
}