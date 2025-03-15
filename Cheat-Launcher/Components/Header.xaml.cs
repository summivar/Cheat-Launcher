using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cheat_Launcher.Components
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        public bool IsSettingsOpen { get; set; }
        public event EventHandler<bool> SettingsToggled;

        public Header(bool openSettings)
        {
            InitializeComponent();
            this.IsSettingsOpen = openSettings;
        }

        private void OpenTelegram(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://t.me") { UseShellExecute = true });
        }

        private void OpenVK(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://vk.com") { UseShellExecute = true });
        }

        private void OpenDiscord(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://discord.com") { UseShellExecute = true });
        }

        private void ToggleSettings(object sender, MouseButtonEventArgs e)
        {
            IsSettingsOpen = !IsSettingsOpen;
            SettingsToggled?.Invoke(this, IsSettingsOpen);
        }

        private void CloseWindow(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}
