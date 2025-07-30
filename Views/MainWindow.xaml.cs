using RetroGamesLauncher.Data.Repositories;
using RetroGamesLauncher.Models;
using RetroGamesLauncher.Models.ViewModels;
using RetroGamesLauncher.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    #region Fields

    private GameInfo selectedGame = null; // Armazena o jogo atual
    private readonly IGameRepository _gameRepository;
    private GlobalHotkeyManager _hotkeyManager;

    private bool _isPnlActionsExpanded = false;
    private double _collapsedHeightPnlActions = 40; // Altura mínima quando colapsado
    private double _expandedHeightPnlActions;       // Variável para armazenar a altura dinâmica

    private int _currentPage = 1;
    private int _totalGamesCount;

    // Variável para armazenar o token de cancelamento da busca
    private CancellationTokenSource _searchCancellationTokenSource;

    ObservableCollection<GameViewModel> _visibleGames = new();
    const int _pageSize = 15;

    #endregion

    #region Constructor

    public MainWindow(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
        InitializeComponent();
        LoadGameList();
        Loaded += Window_Loaded;
        Closing += MainWindow_Closing;
    }

    #endregion

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

    #endregion

    #region GameList

    /// <summary>
    /// Carrega a lista de jogos do repositório e exibe na interface.
    /// </summary>
    private void LoadGameList()
    {
        _totalGamesCount = _gameRepository.GetTotalCount();
        LoadNextPage();
        
        GameListBox.ItemsSource = _visibleGames;

        GameListBox.Loaded += (s, e) =>
        {
            var scrollViewer = GetScrollViewer(GameListBox);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
            else
            {
                Debug.WriteLine("ScrollViewer ainda é null após Loaded");
            }
        };
    }

    private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        var scrollViewer = (ScrollViewer)sender;
        if (scrollViewer.VerticalOffset + scrollViewer.ViewportHeight >= scrollViewer.ExtentHeight - _pageSize)
        {
            LoadNextPage();
        }
    }

    private async void LoadNextPage()
    {
        if ((_currentPage - 1) * _pageSize >= _totalGamesCount)
            return;

        var page = await _gameRepository.GetByPaging(_currentPage, _pageSize);
        foreach (var game in page)
        {
            _visibleGames.Add(new GameViewModel
            {
                Title = game.Title,
                ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + game.ImagePath)),
                Game = game
            });
        }

        _currentPage++;
    }

    private void GameListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (GameListBox.SelectedItem is GameViewModel selectedVm)
        {
            DisplayGame(selectedVm.Game);
        }
    }

    private ScrollViewer GetScrollViewer(DependencyObject depObj)
    {
        if (depObj is ScrollViewer) return (ScrollViewer)depObj;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
            var child = VisualTreeHelper.GetChild(depObj, i);
            var result = GetScrollViewer(child);
            if (result != null) return result;
        }
        return null;
    }

    #endregion

    #region GameDisplay

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

    #endregion

    #region Search

    private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _searchCancellationTokenSource?.Cancel();
        _searchCancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _searchCancellationTokenSource.Token;

        try
        {
            // Implementa o debounce
            await Task.Delay(300, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return;

            string searchText = SearchTextBox.Text.Trim();

            _visibleGames.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                // Volta para a exibição normal (paginada)
                _currentPage = 1;
                _totalGamesCount = _gameRepository.GetTotalCount();
                await LoadNextPageAsync();
            }
            else
            {
                // Busca e exibe todos os resultados correspondentes
                var results = await _gameRepository.GetByTitleLike(searchText);
                foreach (var game in results)
                {
                    _visibleGames.Add(new GameViewModel
                    {
                        Title = game.Title,
                        ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + game.ImagePath)),
                        Game = game
                    });
                }
            }
        }
        catch (TaskCanceledException)
        {
            // Busca cancelada, não faz nada
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocorreu um erro na busca: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Nova versão assíncrona para uso interno na busca
    private async Task LoadNextPageAsync()
    {
        if ((_currentPage - 1) * _pageSize >= _totalGamesCount)
            return;

        var page = await _gameRepository.GetByPaging(_currentPage, _pageSize);
        foreach (var game in page)
        {
            _visibleGames.Add(new GameViewModel
            {
                Title = game.Title,
                ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + game.ImagePath)),
                Game = game
            });
        }

        _currentPage++;
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

    private void BtnAddGame_Click(object sender, RoutedEventArgs e)
    {

    }

    private void BtnSettings_Click(object sender, RoutedEventArgs e)
    {

    }

    #endregion
}