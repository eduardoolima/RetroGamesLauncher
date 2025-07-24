using RetroGamesLauncher.Data.Repositories;
using RetroGamesLauncher.Models;
using RetroGamesLauncher.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RetroGamesLauncher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameInfo selectedGame = null; // Armazena o jogo atual
        private readonly IGameRepository _gameRepository;
        private GlobalHotkeyManager _hotkeyManager;

        public MainWindow(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            InitializeComponent();
            LoadGameList();
            Loaded += Window_Loaded;
            Closing += MainWindow_Closing;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var helper = new WindowInteropHelper(this);
            _hotkeyManager = new GlobalHotkeyManager(helper.Handle);
            _hotkeyManager.HotkeyPressed += HotkeyManager_HotkeyPressed;
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _hotkeyManager?.Dispose();
        }        

        private void HotkeyManager_HotkeyPressed(object sender, EventArgs e)
        {
            if(selectedGame != null)
                EmulatorManager.CloseEmulator();
        }

        //private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //        DragMove(); // permite mover a janela
        //}

        //private void Minimize_Click(object sender, RoutedEventArgs e)
        //{
        //    WindowState = WindowState.Minimized;
        //}

        //private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
        //{
        //    if (WindowState == WindowState.Maximized)
        //        WindowState = WindowState.Normal;
        //    else
        //        WindowState = WindowState.Maximized;
        //}

        //private void Close_Click(object sender, RoutedEventArgs e)
        //{
        //    Close();
        //}

        //// Visual extra: muda a cor do botão fechar ao passar o mouse
        //private void Close_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    (sender as Button).Background = Brushes.Red;
        //}
        //private void Close_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    (sender as Button).Background = Brushes.Transparent;
        //}

        private void LoadGameList()
        {           
            var games = _gameRepository.GetAll();
            foreach (var game in games)
            {
                try
                {
                    var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };

                    var imgPath = AppDomain.CurrentDomain.BaseDirectory + game.ImagePath;
                    var img = new Image
                    {
                        Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute)),
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

            var imgPath = AppDomain.CurrentDomain.BaseDirectory + game.ScreenshotPath;
            ImgMainImage.Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute));
            TxtGameTitle.Text = game.Title;
            TxtGameDescription.Text = game.Description;

            BtnPlayButton.Visibility = Visibility.Visible;
            // Aqui você pode também adicionar botão para jogar
        }

        private void BtnPlayButton_Click(object sender, RoutedEventArgs e)
        {           
            if (selectedGame == null) return;
            
            EmulatorManager.LaunchEmulator(selectedGame.EmulatorId, selectedGame.RomPath);
            //await Task.Delay(2500);
            //ShowTemporaryNotification("Pressione Shift + Esc para fechar o emulador");

        }


    }
}