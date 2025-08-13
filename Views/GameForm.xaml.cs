using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using RetroGamesLauncher.Data;
using RetroGamesLauncher.Data.Repositories;
using RetroGamesLauncher.Models;
using RetroGamesLauncher.Models.AuxModels;
using RetroGamesLauncher.Models.Enums;
using RetroGamesLauncher.Services;
using RetroGamesLauncher.Views.Shared;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RetroGamesLauncher.Views
{
    /// <summary>
    /// Lógica interna para GameForm.xaml
    /// </summary>
    public partial class GameForm : Window
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameGenderRepository _gameGenderRepository;
        private List<GameGender> _genders;

        // Variáveis de nível de classe para armazenar os caminhos das imagens
        private string _gameCoverPath;
        private string _gameCoverPathOriginal;
        private string _gameScreenshotPath;
        private string _gameScreenshotPathOriginal;
        public GameForm(IGameRepository gameRepository, IGameGenderRepository gameGenderRepository)
        {
            InitializeComponent();
            _gameRepository = gameRepository;
            _gameGenderRepository = gameGenderRepository;

            LoadGenders();
            LoadEmulatorCombobox();
        }

        #region Métodos de Inicialização
        void LoadEmulatorCombobox()
        {
            var enumItems = Enum.GetNames(typeof(Emulators))
                .Where(name => name != "WithoutEmulator")
                .Select(name => new EnumItem
                {
                    Name = name,
                    Value = (int)Enum.Parse(typeof(Emulators), name)
                }).ToList();
            EmulatorComboBox.ItemsSource = enumItems;
            if (enumItems.Count < 2)
            {
                EmulatorComboBox.SelectedIndex = 0;
            }
        }
        void LoadGenders()
        {
            _genders = _gameGenderRepository.GetAll().OrderBy(g => g.Gender).ToList();
            GenderComboBox.ItemsSource = _genders;
        }
        #endregion

        #region Eventos de Interface
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GameInfoValidation.FormValidate(this))
                {
                    var selectedEmulatorItem = EmulatorComboBox.SelectedItem as EnumItem;

                    #region Cópias de arquivos
                    #region Rom
                    string extension = Path.GetExtension(RomPathTextBox.Text);
                    string newRomFileName = Guid.NewGuid().ToString() + extension;
                    string newRomPath = App.Configuration["FilePaths:Roms"];
                    newRomPath = Path.Combine(newRomPath, newRomFileName);
                    File.Copy(RomPathTextBox.Text, newRomPath);
                    #endregion
                    #region Imagens

                    if (_gameCoverPathOriginal != null && _gameCoverPath != null)
                    {
                        File.Copy(_gameCoverPathOriginal, _gameCoverPath, true);
                    }
                    if (_gameScreenshotPathOriginal != null && _gameScreenshotPath != null)
                    {
                        File.Copy(_gameScreenshotPathOriginal, _gameScreenshotPath, true);
                    }  
                    #endregion
                    #endregion

                    var newGame = new GameInfo
                    {
                        Title = TitleTextBox.Text,
                        Description = DescriptionTextBox.Text,
                        RomPath = newRomPath,
                        ImagePath = _gameCoverPath,
                        ScreenshotPath = _gameScreenshotPath,
                        Gender = GenderComboBox.SelectedItem as GameGender,
                        EmulatorId = (Emulators)selectedEmulatorItem?.Value
                    };                                        
                    _gameRepository.Add(newGame);
                    ToastMessages.ShowTemporaryNotification("✔️ Jogo salvo com sucesso!", TypeToastMessage.Success);
                    Close();
                }
            }
            catch (Exception)
            {
                ToastMessages.ShowTemporaryNotification("☠ Deu Ruim!", TypeToastMessage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();

        private void AddGender_Click(object sender, RoutedEventArgs e)
        {
            var gameFormGenderWindow = App.Services.GetRequiredService<GameGenderForm>();
            gameFormGenderWindow.Owner = this;
            gameFormGenderWindow.GenderAdded += OnGenderAdded;
            gameFormGenderWindow.Show();
        }

        private async void BtnAddGameCover_Click(object sender, RoutedEventArgs e)
        {
            await LoadImageAndSetSource(ImgGameCoverViewer);
        }

        private async void BtnAddGameScreenshot_Click(object sender, RoutedEventArgs e)
        {
            await LoadImageAndSetSource(ImgGameScreenshotViewer);
        }

        private void BtnAddRom_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Arquivos de ROM|*.smc;*.sfc;*.gen;*.md;*.gba;*.nes|Todos os Arquivos|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                RomPathTextBox.Text = openFileDialog.FileName;
            }
        } 
        #endregion

        #region Manipulação de Imagens
        private async Task LoadImageAndSetSource(Image imageViewer)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Arquivos de Imagem|*.jpg;*.jpeg;*.png;*.webp;*";

            if (openFileDialog.ShowDialog() == true)
            {
                string originalPath = openFileDialog.FileName;
                try
                {
                    string extension = Path.GetExtension(originalPath);
                    string newFileName = Guid.NewGuid().ToString() + extension;
                    string newPath = App.Configuration["FilePaths:Images"];
                    if (imageViewer == ImgGameCoverViewer)
                    {
                        newPath = Path.Combine(newPath, @$"GamesCover\{newFileName}");
                        _gameCoverPath = newPath;
                        _gameCoverPathOriginal = originalPath;
                    }
                    else if (imageViewer == ImgGameScreenshotViewer)
                    {
                        newPath = Path.Combine(newPath, @$"GamesScreenshot\{newFileName}");
                        _gameScreenshotPath = newPath;
                        _gameScreenshotPathOriginal = originalPath;
                    }

                    BitmapImage bitmap = await Task.Run(() =>
                    {
                        BitmapImage bmp = new();
                        bmp.BeginInit();
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.UriSource = new Uri(originalPath);
                        bmp.EndInit();
                        bmp.Freeze();
                        //File.Copy(originalPath, newPath);

                        return bmp;
                    });
                    imageViewer.Source = bitmap;
                }
                catch (Exception ex)
                {
                    if (imageViewer == ImgGameCoverViewer)
                    {
                        _gameCoverPath = null;                        
                        _gameCoverPathOriginal = null;
                    }
                    else if (imageViewer == ImgGameScreenshotViewer)
                    {
                        _gameScreenshotPath = null;
                        _gameScreenshotPathOriginal = null;
                    }
                    imageViewer.Source = null;
                    MessageBox.Show("Erro ao carregar a imagem: " + ex.Message);
                }
            }
        }         
        #endregion

        #region Métodos Auxiliares
        private void OnGenderAdded(object sender, EventArgs e)
        {
            LoadGenders();
        } 
        #endregion
    }
}
