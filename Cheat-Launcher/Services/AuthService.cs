using Cheat_Launcher.Constants;
using Cheat_Launcher.Data.SecureStorage;
using Cheat_Launcher.Dtos.Requests.Auth;
using Cheat_Launcher.Dtos.Responses.Auth;
using Cheat_Launcher.Utils;
using RestSharp;

namespace Cheat_Launcher.Services;

public class AuthResponse
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public AuthResponse(bool status, string message)
    {
        Status = status;
        Message = message;
    }
}

public class AuthService
{
    private readonly RestClient _restClient;
    private readonly ISecureStorage _secureStorage;

    public AuthService(RestClient restClient, ISecureStorage secureStorage)
    {
        _restClient = restClient;
        _secureStorage = secureStorage;
    }

    public async Task<AuthResponse> Register(string email, string username, string password,
        string? referral)
    {
        var hwid = SystemGuid.Value();

        var registerRequest = new AuthLoginRequestDto
        {
            Email = email,
            Username = username,
            Password = password,
            Hwid = hwid,
            Referral = referral
        };

        var request = new RestRequest("auth/register", Method.Post);
        request.AddJsonBody(registerRequest);

        var response = await _restClient.ExecuteAsync<AuthRegisterResponseDto>(request);

        if (!response.IsSuccessful)
        {
            var message = HandlerErrorResponse.Handle(response);
            return new AuthResponse(false, message);
        }

        _secureStorage.Save(KeyConstants.JwtToken, response.Data!.Token);
        _secureStorage.Save(KeyConstants.User, response.Data!.User!);

        return new AuthResponse(true, "Authentication success");
    }

    public async Task<AuthResponse> Login(string email, string password)
    {
        var hwid = SystemGuid.Value();

        var loginRequest = new AuthLoginRequestDto
        {
            Email = email,
            Password = password,
            Hwid = hwid,
        };

        var request = new RestRequest("auth/login", Method.Post);
        request.AddJsonBody(loginRequest);

        var response = await _restClient.ExecuteAsync<AuthLoginResponseDto>(request);

        if (!response.IsSuccessful)
        {
            var message = HandlerErrorResponse.Handle(response);
            return new AuthResponse(false, message);
        }

        _secureStorage.Save(KeyConstants.JwtToken, response.Data!.Token);
        _secureStorage.Save(KeyConstants.User, response.Data!.User!);

        return new AuthResponse(true, "Authentication success");
    }

    public async Task<bool> CheckAuthAsync()
    {
        try
        {
            var request = new RestRequest("auth/check", Method.Post);

            var response = await _restClient.PostAsync(request);

            return response.IsSuccessful;
        }
        catch
        {
            return false;
        }
    }
}