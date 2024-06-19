using Newtonsoft.Json;

namespace DemoLibrary.Application.Dtos.Blacklist;

public class CreateBlacklistDto
{
    [JsonProperty("jti")] 
    public string Jti { get; set; }

    private DateTime _expiryDate;

    [JsonProperty("exp")]
    public long ExpiryDateUnix
    {
        set { _expiryDate = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime; }
    }

    [JsonIgnore]
    public DateTime ExpiryDate
    {
        get { return _expiryDate; }
    }
}