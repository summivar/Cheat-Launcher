using Newtonsoft.Json;

namespace Cheat_Launcher.Dtos.Responses.Games;

public class Photo
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("path")] public string Path { get; set; }
    [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updatedAt")] public DateTime UpdatedAt { get; set; }
}

public class FindGameResponseDto
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("description")] public string Description { get; set; }

    [JsonProperty("status")] public string Status { get; set; }

    [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }

    [JsonProperty("updatedAt")] public DateTime UpdatedAt { get; set; }
    
    [JsonProperty("remainingTime")] public long RemainingTime { get; set; }

    [JsonProperty("photo")] public Photo Photo { get; set; }
}