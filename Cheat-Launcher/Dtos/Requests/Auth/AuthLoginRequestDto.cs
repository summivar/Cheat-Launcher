using Newtonsoft.Json;

namespace Cheat_Launcher.Dtos.Requests.Auth;

public class AuthLoginRequestDto
{
    [JsonProperty("email")] public string Email { get; set; }

    [JsonProperty("username")] public string Username { get; set; }

    [JsonProperty("password")] public string Password { get; set; }

    [JsonProperty("hwid")] public string Hwid { get; set; }

    [JsonProperty("referralId")] public string? Referral { get; set; }
}