using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using WpfApp4.View;
using WpfApp4.StartUpHelper;

namespace WpfApp4
{

    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                   .ConfigureServices((hostcontex, services) =>
                   {
                       services.AddSingleton<MainWindow>();
                       services.AddFormFactory<GridDB>();
                   }).Build();

        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StopAsync();
            var startForm = AppHost.Services.GetRequiredService<MainWindow>();
            startForm.Show();
            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }


}
