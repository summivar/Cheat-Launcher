using System.Windows;
using Cheat_Launcher.Components;
using Cheat_Launcher.Constants;
using Cheat_Launcher.Data.SecureStorage;
using Cheat_Launcher.Dtos.Responses.Games;
using Cheat_Launcher.Services;

namespace Cheat_Launcher.Windows
{
    public partial class MainWindow : Window
    {
        private GamesService _gamesService;
        private KeyService _keyService;
        private ISecureStorage _secureStorage;
        private string Username { get; set; } = "User";
        public List<FindGameResponseDto> Games { get; set; } = [];
        public FindGameResponseDto SelectedGame { get; set; }

        public bool OpenSettings { get; set; } = false;

        public MainWindow(GamesService gamesService, ISecureStorage secureStorage, KeyService keyService)
        {
            InitializeComponent();
            _gamesService = gamesService;
            _secureStorage = secureStorage;
            _keyService = keyService;

            var headerControl = new Header(OpenSettings);
            headerControl.SettingsToggled += HeaderControl_SettingsToggled;
            HeaderContainer.Children.Add(headerControl);

            Username = _secureStorage.Load(KeyConstants.UserName);

            LoadGamesAsync();
        }

        private async void LoadGamesAsync()
        {
            try
            {
                Games = await _gamesService.FindGames();
                if (Games.Count > 0)
                {
                    SelectedGame = Games[0];
                }

                DisplayGameInfo(SelectedGame);

                var gamesListControl = new GamesList(Games, SelectedGame);
                gamesListControl.GameSelected += GamesListControl_GameSelected;
                GamesListContainer.Children.Add(gamesListControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading games: {ex.Message}");
            }
        }

        private void DisplayGameInfo(FindGameResponseDto game)
        {
            if (OpenSettings)
            {
                var settingsControl = new Settings(Username);
                settingsControl.KeyEntered += Settings_KeyEntered;

                MainContainer.Children.Clear();
                MainContainer.Children.Add(settingsControl);
            }
            else
            {
                var gameInfoControl = new GameInfo();
                gameInfoControl.UpdateGameInfo(game);

                MainContainer.Children.Clear();
                MainContainer.Children.Add(gameInfoControl);
            }
        }

        private void GamesListControl_GameSelected(object sender, FindGameResponseDto selectedGame)
        {
            SelectedGame = selectedGame;
            OpenSettings = false;
            DisplayGameInfo(SelectedGame);
        }

        private void HeaderControl_SettingsToggled(object sender, bool e)
        {
            OpenSettings = e;
            DisplayGameInfo(SelectedGame);
        }

        private async void Settings_KeyEntered(object sender, string activationKey)
        {
            var response = await _keyService.KeyActivate(activationKey);
            MessageBox.Show(response.Message);
            if (response.Status)
            {
                await ReloadGamesAsync();
            }
        }

        private async Task ReloadGamesAsync()
        {
            Games = await _gamesService.FindGames();
            if (Games.Count > 0)
            {
                SelectedGame = Games[0];
            }

            DisplayGameInfo(SelectedGame);

            GamesListContainer.Children.Clear();
            var gamesListControl = new GamesList(Games, SelectedGame);
            gamesListControl.GameSelected += GamesListControl_GameSelected;
            GamesListContainer.Children.Add(gamesListControl);
        }
    }
}
