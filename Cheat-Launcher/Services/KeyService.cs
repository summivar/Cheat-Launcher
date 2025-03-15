using Cheat_Launcher.Dtos.Responses.Keys;
using Cheat_Launcher.Utils;
using RestSharp;

namespace Cheat_Launcher.Services;

public class KeyResponse
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public KeyResponse(bool status, string message)
    {
        Status = status;
        Message = message;
    }
}

public class KeyService
{
    private readonly RestClient _restClient;

    public KeyService(RestClient restClient)
    {
        _restClient = restClient;
    }

    public async Task<KeyResponse> KeyActivate(string key)
    {
        var request = new RestRequest("user/key/" + key, Method.Get);

        var response = await _restClient.ExecuteAsync<ActivateKeyResponseDto>(request);

        if (!response.IsSuccessful)
        {
            var message = HandlerErrorResponse.Handle(response);
            return new KeyResponse(false, message);
        }

        return new KeyResponse(true, "Ключ активирован");
    }
}