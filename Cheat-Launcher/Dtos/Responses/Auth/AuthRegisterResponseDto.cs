using Newtonsoft.Json;

namespace Cheat_Launcher.Dtos.Responses.Auth;

public class AuthRegisterResponseDto
{
    [JsonProperty("token")] public string Token { get; set; }

    [JsonProperty("user")] public AuthUserResponseDto User { get; set; }
}