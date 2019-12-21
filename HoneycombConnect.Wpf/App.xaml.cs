using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using System.Windows;

namespace HoneycombConnect.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; private set; }

        private MainWindow mainWindow = null;

        public IConfigurationRoot Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Loaded += MainWindow_Loaded;
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File("honeycombconnect.log").CreateLogger();

            services.AddOptions();
            //services.Configure<AppSettings>((appSettings) =>
            //{
            //    Configuration.GetSection("AppSettings").Bind(appSettings);
            //});
            services.AddLogging(configure =>
            {
                configure
                    .AddDebug()
                    .AddSerilog();
            });

            services.AddTransient(typeof(MainWindow));
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
