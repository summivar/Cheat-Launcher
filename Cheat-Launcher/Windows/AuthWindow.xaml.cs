using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Cheat_Launcher.Services;
using Microsoft.Web.WebView2.Core;

namespace Cheat_Launcher.Windows
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private AuthService _authService;
        private readonly Func<MainWindow> _mainWindowFactory;
        public AuthWindow(AuthService authService, Func<MainWindow> mainWindowFactory)
        {
            _mainWindowFactory = mainWindowFactory;
            _authService = authService;
            InitializeComponent();
            InitializeWebView();
        }
        private async void InitializeWebView()
        {
            string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Renderer/Login", "index.html");
            Console.WriteLine("HTML Path: " + htmlPath);

            if (File.Exists(htmlPath))
            {
                await webView.EnsureCoreWebView2Async(null);
                webView.CoreWebView2.Navigate($"file:///{htmlPath.Replace("\\", "/")}");

                webView.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;
            }
            else
            {
                MessageBox.Show("Файл не найден");
            }
        }

        private async void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var message = e.TryGetWebMessageAsString();
            var messageData = JsonSerializer.Deserialize<Dictionary<string, string>>(message);

            if (messageData != null && messageData.ContainsKey("type") && messageData["type"] == "register")
            {
                string email = messageData["email"];
                string username = messageData["username"];
                string password = messageData["password"];

                var response = await _authService.Register(email, username, password, null);
                await SendResponseToWebView(response);

                if (response.Status)
                {
                    OpenMainWindow();
                }
                return;
            }

            if (messageData != null && messageData.ContainsKey("type") && messageData["type"] == "login")
            {
                string email = messageData["email"];
                string password = messageData["password"];

                var response = await _authService.Login(email, password);
                await SendResponseToWebView(response);

                if (response.Status)
                {
                    OpenMainWindow();
                }
                return;
            }

            if (messageData != null && messageData.ContainsKey("type") && messageData["type"] == "navigateToRegister")
            {
                NavigateToRegisterPage();
                return;
            }

            if (messageData != null && messageData.ContainsKey("type") && messageData["type"] == "navigateToLogin")
            {
                NavigateToLoginPage();
                return;
            }
        }

        private async Task SendResponseToWebView(AuthResponse response)
        {
            var message = new
            {
                type = response.Status ? "authSuccess" : "authError",
                message = response.Message
            };

            string jsonMessage = JsonSerializer.Serialize(message);
            webView.CoreWebView2.PostWebMessageAsString(jsonMessage);
        }

        private void OpenMainWindow()
        {
            var mainWindow = _mainWindowFactory.Invoke();
            mainWindow.Show();
            webView.CoreWebView2.Stop();
            this.Close();
        }

        private void NavigateToRegisterPage()
        {
            string registerPagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Renderer/Register", "index.html");
            if (File.Exists(registerPagePath))
            {
                webView.CoreWebView2.Navigate($"file:///{registerPagePath.Replace("\\", "/")}");
            }
            else
            {
                MessageBox.Show("Файл не найден");
            }
        }

        private void NavigateToLoginPage()
        {
            string loginPagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Renderer/Login", "index.html");
            if (File.Exists(loginPagePath))
            {
                webView.CoreWebView2.Navigate($"file:///{loginPagePath.Replace("\\", "/")}");
            }
            else
            {
                MessageBox.Show("Файл не найден");
            }
        }
    }
}
