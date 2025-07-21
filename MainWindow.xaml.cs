using RetroGamesLauncher.Data;
using RetroGamesLauncher.Data.Repositories;
using RetroGamesLauncher.Models;
using RetroGamesLauncher.Services;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RetroGamesLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameInfo selectedGame = null; // Armazena o jogo atual
        private readonly IGameRepository _gameRepository;

        public MainWindow(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            InitializeComponent();
            LoadGameList();
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //_emulatorManager.CleanupExtractedEmulators();
        }


        private void LoadGameList()
        {           
            var games = _gameRepository.GetAll();
            foreach (var game in games)
            {
                try
                {
                    var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };

                    var img = new Image
                    {
                        Source = new BitmapImage(new Uri(game.ImagePath, UriKind.Relative)),
                        Width = 60,
                        Height = 60,
                        Margin = new Thickness(5)
                    };

                    var label = new TextBlock
                    {
                        Text = game.Title,
                        VerticalAlignment = VerticalAlignment.Center,
                        Foreground = Brushes.White,
                        Margin = new Thickness(10, 0, 0, 0),
                        FontSize = 16
                    };

                    panel.Children.Add(img);
                    panel.Children.Add(label);
                    panel.MouseLeftButtonUp += (s, e) => DisplayGame(game);

                    PnlGameList.Children.Add(panel);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void DisplayGame(GameInfo game)
        {
            selectedGame = game;

            ImgMainImage.Source = new BitmapImage(new Uri(game.ScreenshotPath, UriKind.Relative));
            TxtGameTitle.Text = game.Title;
            TxtGameDescription.Text = game.Description;

            BtnPlayButton.Visibility = Visibility.Visible;
            // Aqui você pode também adicionar botão para jogar
        }

        private void BtnPlayButton_Click(object sender, RoutedEventArgs e)
        {           
            if (selectedGame == null) return;

            EmulatorManager.LaunchEmulator(selectedGame.EmulatorId, selectedGame.RomPath);

        }

    }
}