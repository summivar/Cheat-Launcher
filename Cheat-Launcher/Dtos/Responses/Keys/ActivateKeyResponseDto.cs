using Newtonsoft.Json;

namespace Cheat_Launcher.Dtos.Responses.Keys;

public class ActivateKeyResponseDto
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("description")] public string Description { get; set; }

    [JsonProperty("expiredAt")] public DateTime ExpiredAt { get; set; }
}