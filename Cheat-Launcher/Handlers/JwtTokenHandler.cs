using System.Net.Http;
using Cheat_Launcher.Constants;
using Cheat_Launcher.Data.SecureStorage;

namespace Cheat_Launcher.Handlers;

public class JwtTokenHandler : DelegatingHandler
{
    private readonly ISecureStorage _secureStorage;

    public JwtTokenHandler(ISecureStorage secureStorage)
    {
        _secureStorage = secureStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            var jwtToken = _secureStorage.Load<string>(KeyConstants.JwtToken);
            if (!string.IsNullOrEmpty(jwtToken))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
            }
        }
        catch
        {
            // ignore
        }

        return await base.SendAsync(request, cancellationToken);
    }
}