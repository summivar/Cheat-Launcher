using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cheat_Launcher.Dtos.Responses.Games;

namespace Cheat_Launcher.Components
{
    /// <summary>
    /// Interaction logic for GameInfo.xaml
    /// </summary>
    public partial class GameInfo : UserControl
    {
        public FindGameResponseDto? SelectedGame { get; set; }

        public GameInfo()
        {
            InitializeComponent();
        }

        public void UpdateGameInfo(FindGameResponseDto? game)
        {
            SelectedGame = game;
            if (game != null)
            {
                GameDescriptionTextBlock.Text = game.Description; // Привязываем описание игры
                GameImage.Source = new BitmapImage(new Uri(game.Photo.Path)); // Привязываем изображение игры
            }
        }
    }

}
