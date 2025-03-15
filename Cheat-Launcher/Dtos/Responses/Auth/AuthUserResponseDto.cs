using Newtonsoft.Json;

namespace Cheat_Launcher.Dtos.Responses.Auth;

public class AuthUserResponseDto
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("email")] public string Email { get; set; }

    [JsonProperty("username")] public string Username { get; set; }

    [JsonProperty("accountStatus")] public string AccountStatus { get; set; }

    [JsonProperty("balance")] public int Balance { get; set; }

    [JsonProperty("role")] public string Role { get; set; }

    [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }

    [JsonProperty("lastLogin")] public DateTime? LastLogin { get; set; }

    [JsonProperty("registrationIp")] public string RegistrationIp { get; set; }

    [JsonProperty("comment")] public string Comment { get; set; }
}