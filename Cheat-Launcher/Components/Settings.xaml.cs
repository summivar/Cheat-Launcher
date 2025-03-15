using System.Windows;
using System.Windows.Controls;

namespace Cheat_Launcher.Components
{
    public partial class Settings : UserControl
    {
        public string Username { get; set; }

        public event EventHandler<string> KeyEntered;

        public Settings(string username)
        {
            InitializeComponent();

            Username = username;

            this.DataContext = this;
        }

        private void EnterKeyButton_Click(object sender, RoutedEventArgs e)
        {
            string activationKey = KeyTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(activationKey))
            {
                KeyEntered?.Invoke(this, activationKey);
            }
        }
    }
}
