using Newtonsoft.Json;

namespace Cheat_Launcher.Dtos.Responses.Auth;

public class AuthCheckAuthResponseDto
{
    [JsonProperty("state")] public Boolean State { get; set; }
}