using System.Windows.Controls;
using Cheat_Launcher.Dtos.Responses.Games;

namespace Cheat_Launcher.Components
{
    public partial class GamesList : UserControl
    {
        public event EventHandler<FindGameResponseDto> GameSelected;
        public List<FindGameResponseDto> Games { get; set; }
        public FindGameResponseDto SelectedGame { get; set; }

        public GamesList(List<FindGameResponseDto> games, FindGameResponseDto selectedGame)
        {
            InitializeComponent();
            Games = games;
            SelectedGame = selectedGame;

            GamesListBox.ItemsSource = Games;

            GamesListBox.SelectedItem = SelectedGame;
        }

        private void GamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GamesListBox.SelectedItem is FindGameResponseDto selectedGame)
            {
                SelectedGame = selectedGame;
                GameSelected?.Invoke(this, selectedGame);
            }
        }
    }
}
