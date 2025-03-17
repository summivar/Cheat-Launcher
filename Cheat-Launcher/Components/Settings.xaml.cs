using System.Windows;
using System.Windows.Controls;
using Cheat_Launcher.Constants;
using Cheat_Launcher.Data.SecureStorage;
using Cheat_Launcher.Dtos.Responses.Auth;

namespace Cheat_Launcher.Components
{
    public partial class Settings : UserControl
    {
        private ISecureStorage _secureStorage;
        public string Username { get; set; }
        public AuthUserResponseDto User { get; set; }

        public event EventHandler<string> KeyEntered;

        public Settings(ISecureStorage secureStorage)
        {
            InitializeComponent();

            _secureStorage = secureStorage;

            User = _secureStorage.Load<AuthUserResponseDto>(KeyConstants.User) ?? throw new Exception("User not found");

            Username = User.Username;

            this.DataContext = this;

            if (User?.Role != "tester")
            {
                CheatVersionPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void EnterKeyButton_Click(object sender, RoutedEventArgs e)
        {
            string activationKey = KeyTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(activationKey))
            {
                KeyEntered?.Invoke(this, activationKey);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _secureStorage.Delete(KeyConstants.JwtToken);
            _secureStorage.Delete(KeyConstants.User);

            Application.Current.Shutdown();
        }
    }
}
