using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;
using Cheat_Launcher.Windows;
using Microsoft.Extensions.DependencyInjection;
using Cheat_Launcher.Data.SecureStorage;
using RestSharp;
using Cheat_Launcher.Handlers;
using Cheat_Launcher.Services;
using Cheat_Launcher.Components;

namespace Cheat_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var authService = ServiceProvider.GetRequiredService<AuthService>();

            var isAuth = await authService.CheckAuthAsync();

            if (!isAuth)
            {
                var authWindow = ServiceProvider.GetRequiredService<AuthWindow>();
                authWindow.Show();
                return;
            }

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var apiUrl = configuration["ApiSettings:BaseUrl"] ?? throw new Exception("ApiSettings not found");

            var storagePath = Path.Combine(Directory.GetCurrentDirectory(), "SecureStorage");
            services.AddSingleton<ISecureStorage>(provider => new DpapiSecureStorage(storagePath));

            services.AddSingleton<RestClient>(provider =>
            {
                var secureStorage = provider.GetRequiredService<ISecureStorage>();
                var options = new RestClientOptions(apiUrl)
                {
                    ThrowOnAnyError = false,
                    FailOnDeserializationError = false,
                    ConfigureMessageHandler = handler => new JwtTokenHandler(secureStorage) { InnerHandler = handler }
                };

                var client = new RestClient(options);

                client.AddDefaultHeader("Content-Type", "application/json");

                return client;
            });
            services.AddSingleton<AuthService>();
            services.AddSingleton<KeyService>();
            services.AddSingleton<GamesService>();

            services.AddSingleton<Header>();
            services.AddSingleton<GamesList>();

            services.AddSingleton<AuthWindow>();
            services.AddSingleton<MainWindow>();

            services.AddSingleton<Func<MainWindow>>(provider => () => provider.GetRequiredService<MainWindow>());

            services.AddSingleton<IConfiguration>(configuration);
        }
    }

}
