using Cheat_Launcher.Constants;
using Cheat_Launcher.Data.SecureStorage;
using Cheat_Launcher.Dtos.Responses.Games;
using Cheat_Launcher.Utils;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Cheat_Launcher.Services
{
    public class GamesService
    {
        private readonly RestClient _restClient;
        private readonly IConfiguration _configuration;

        public GamesService(RestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task<List<FindGameResponseDto>> FindGames()
        {
            var imageBaseUrl = _configuration["ApiSettings:ImageBaseUrl"];

            var request = new RestRequest("user/game", Method.Get);

            var response = await _restClient.ExecuteAsync<List<FindGameResponseDto>>(request);

            if (!response.IsSuccessful)
            {
                throw new Exception("Error fetching games.");
            }

            var games = response.Data! ?? [];

            foreach (var game in games)
            {
                if (game.Photo != null)
                {
                    game.Photo.Path = imageBaseUrl + game.Photo.Path;
                }

                if (game.Status == "unDetected")
                {
                    if (game.RemainingTime > 0)
                    {
                        game.Status = "Работает";
                    }
                    else
                    {
                        game.Status = "Не активировано";
                    }
                }
                else if (game.Status == "onUpdate")
                {
                    game.Status = "На обновлении";
                }
                else if (game.Status == "detected")
                {
                    game.Status = "Отключен";
                }
            }

            return games;
        }
    }
}
