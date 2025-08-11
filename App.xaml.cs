using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetroGamesLauncher.Data;
using RetroGamesLauncher.Data.Repositories;
using RetroGamesLauncher.Services;
using RetroGamesLauncher.Views;
using System.IO;
using System.Windows;

namespace RetroGamesLauncher;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost _host;    
    public static IConfiguration Configuration { get; private set; }
    public static IServiceProvider Services => ((App)Current)._host.Services;
    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<AppDbContext>();
                services.AddScoped<IGameRepository, GameRepository>();
                services.AddScoped<IGameGenderRepository, GameGenderRepository>();
                services.AddScoped<MainWindow>();
                services.AddScoped<AddGame>();
            })
            .Build();
    }    

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        // Popular banco se necessário
        using (var scope = _host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            SeedDb.PopulateDbInitialData(db);
        }

        // Mostrar MainWindow com DI
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}
