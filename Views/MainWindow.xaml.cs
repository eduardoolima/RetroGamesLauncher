using RetroGamesLauncher.Data.Repositories;
using RetroGamesLauncher.Models;
using RetroGamesLauncher.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace RetroGamesLauncher.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private GameInfo selectedGame = null; // Armazena o jogo atual
    private readonly IGameRepository _gameRepository;
    private GlobalHotkeyManager _hotkeyManager;

    private bool _isPnlActionsExpanded = false;
    private double _collapsedHeightPnlActions = 40; // Altura mínima quando colapsado
    private double _expandedHeightPnlActions;       // Variável para armazenar a altura dinâmica
    // Variável para armazenar o token de cancelamento da busca
    private CancellationTokenSource _searchCancellationTokenSource;

    public MainWindow(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
        InitializeComponent();
        LoadGameList();
        Loaded += Window_Loaded;
        Closing += MainWindow_Closing;
    }

    #region WindowActions
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var helper = new WindowInteropHelper(this);
        _hotkeyManager = new GlobalHotkeyManager(helper.Handle);
        _hotkeyManager.HotkeyPressed += HotkeyManager_HotkeyPressed;

        // Captura a altura natural do conteúdo do painel quando ele está no seu estado "expandido"
        // Garante que o painel esteja com seu conteúdo visível para medir
        PnlActions.Height = Double.NaN; // Permite que o painel calcule sua altura natural
        PnlActions.Measure(new Size(PnlActions.ActualWidth, Double.PositiveInfinity));
        _expandedHeightPnlActions = PnlActions.DesiredSize.Height;

        PnlActions.Height = _collapsedHeightPnlActions;
    }
    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _hotkeyManager?.Dispose();
    }
    /// <summary>
    /// Carrega a lista de jogos do repositório e exibe na interface.
    /// </summary>
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
                panel.MouseEnter += (s, e) =>
                {
                    panel.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                };
                panel.MouseLeave += (s, e) =>
                {
                    panel.Background = Brushes.Transparent;
                };
                panel.MouseLeftButtonUp += (s, e) => DisplayGame(game);

                PnlGameList.Children.Add(panel);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    /// <summary>
    /// Exibe as informações do jogo selecionado na interface.
    /// </summary>
    /// <param name="game"></param>
    private void DisplayGame(GameInfo game)
    {
        selectedGame = game;

        var imgPath = AppDomain.CurrentDomain.BaseDirectory + game.ScreenshotPath;
        ImgMainImage.Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute));
        TxtGameTitle.Text = game.Title;
        TxtGameDescription.Text = game.Description;

        BtnPlayButton.Visibility = Visibility.Visible;
    }
    private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _searchCancellationTokenSource?.Cancel();
        _searchCancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _searchCancellationTokenSource.Token;

        // Pega o texto atual da caixa de busca
        string searchText = SearchTextBox.Text;
        try
        {
            // Implementa o debounce
            // Espera por 300 milissegundos antes de disparar a busca
            await Task.Delay(300, cancellationToken);

            // Se a busca foi cancelada durante o delay (usuário digitou novamente), retorna
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            // Agora, faz a busca no banco de dados
            //await PerformSearch(searchText, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            // Isso é esperado quando uma busca é cancelada, então não precisa fazer nada.
            // Significa que o usuário digitou mais rápido do que o delay.
        }
        catch (Exception ex)
        {
            // Tratar outros erros que possam ocorrer durante a busca
            MessageBox.Show($"Ocorreu um erro na busca: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    #region ButtonsAndHotkeys
    private void HotkeyManager_HotkeyPressed(object sender, EventArgs e)
    {
        if (selectedGame != null)
            EmulatorManager.CloseEmulator();
    }
    private void TogglePanel_Click(object sender, RoutedEventArgs e)
    {
        var animation = new DoubleAnimation
        {
            From = _isPnlActionsExpanded ? _expandedHeightPnlActions : _collapsedHeightPnlActions,
            To = _isPnlActionsExpanded ? _collapsedHeightPnlActions : _expandedHeightPnlActions,
            Duration = new Duration(TimeSpan.FromMilliseconds(300)),
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
        };

        PnlActions.BeginAnimation(HeightProperty, animation);

        _isPnlActionsExpanded = !_isPnlActionsExpanded;
        BtnTogglePanel.Content = _isPnlActionsExpanded ? "⎯" : "☰";
    }
    private void BtnPlayButton_Click(object sender, RoutedEventArgs e)
    {
        if (selectedGame == null) return;

        EmulatorManager.LaunchEmulator(selectedGame.EmulatorId, selectedGame.RomPath);
        //await Task.Delay(2500);
        //ShowTemporaryNotification("Pressione Shift + Esc para fechar o emulador");

    }
    #endregion

    private void BtnAddGame_Click(object sender, RoutedEventArgs e)
    {

    }

    private void BtnSettings_Click(object sender, RoutedEventArgs e)
    {

    }
}