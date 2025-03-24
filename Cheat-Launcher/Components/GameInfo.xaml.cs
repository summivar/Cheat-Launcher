using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Cheat_Launcher.Dtos.Responses.Games;
using Cheat_Launcher.Utils;
using Cheat_Launcher.Windows;
using Microsoft.Win32;

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

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            string dllPath = OpenFileDialogForDll();

            if (string.IsNullOrEmpty(dllPath)) return;

            Process selectedProcess = SelectProcess();

            if (selectedProcess == null) return;


            try {
                Injector.Handle([selectedProcess.Id.ToString(), dllPath]);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private string OpenFileDialogForDll()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DLL files (*.dll)|*.dll|All files (*.*)|*.*";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        private Process SelectProcess()
        {
            Process[] processes = Process.GetProcesses();
            ProcessWindow processWindow = new ProcessWindow(processes);

            if (processWindow.ShowDialog() == true)
            {
                return processWindow.SelectedProcess;
            }

            return null;
        }
    }

}
