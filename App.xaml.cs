using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetroGamesLauncher.Data;
using RetroGamesLauncher.Data.Repositories;
using RetroGamesLauncher.Services;
using RetroGamesLauncher.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetroGamesLauncher;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<AppDbContext>();
                services.AddScoped<IGameRepository, GameRepository>();
                services.AddScoped<MainWindow>();
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

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }

    private void ComboBox_ClickAnywhere(object sender, MouseButtonEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            if (comboBox.IsDropDownOpen)
            {
                comboBox.IsDropDownOpen = false;
                e.Handled = false;
            }
            else
            {
                comboBox.Focus();
                comboBox.IsDropDownOpen = true;
                e.Handled = true;
            }
        }
    }



}
