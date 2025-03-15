using Cheat_Launcher.Dtos.Responses.Errors;
using Newtonsoft.Json;
using RestSharp;

namespace Cheat_Launcher.Utils;

public class HandlerErrorResponse
{
    public static string Handle(RestResponse response)
    {
        var errorMessage = "Unknown error occurred";

        if (response.Content == null)
        {
            return errorMessage;
        }

        try
        {
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
            if (errorResponse?.Message != null)
            {
                errorMessage = errorResponse.Message;
            }
        }
        catch
        {
            // ignore
        }

        return errorMessage;
    }
}