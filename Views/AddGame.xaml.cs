using Microsoft.Win32;
using RetroGamesLauncher.Data;
using RetroGamesLauncher.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RetroGamesLauncher.Views
{
    /// <summary>
    /// Lógica interna para AddGame.xaml
    /// </summary>
    public partial class AddGame : Window
    {
        private readonly List<GameGender> _genders; 
        public AddGame()
        {
            InitializeComponent();
            _genders = LoadGenders(); // Carrega os gêneros do banco/dados
            GenderComboBox.ItemsSource = _genders;
        }

        private List<GameGender> LoadGenders()
        {
            using var db = new AppDbContext(); // Substitua pelo seu contexto real
            return db.GameGenders.OrderBy(g => g.Gender).ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var newGame = new GameInfo
            {
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                RomPath = RomPathTextBox.Text,
                //ImagePath = ImagePathTextBox.Text,
                //ScreenshotPath = ScreenshotPathTextBox.Text,
                Gender = GenderComboBox.SelectedItem as GameGender,
                // EmulatorId: defina como quiser
            };

            using var db = new AppDbContext();
            db.Games.Add(newGame);
            db.SaveChanges();
            MessageBox.Show("Jogo salvo com sucesso!");
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();

        private void AddGender_Click(object sender, RoutedEventArgs e)
        {
            var input = Microsoft.VisualBasic.Interaction.InputBox("Digite o novo gênero:", "Novo Gênero", "");
            if (!string.IsNullOrWhiteSpace(input))
            {
                using var db = new AppDbContext();
                var gender = new GameGender(input);
                db.GameGenders.Add(gender);
                db.SaveChanges();

                _genders.Add(gender);
                GenderComboBox.Items.Refresh();
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem;
                //MessageBox.Show($"Selecionado: {selectedItem}");
            }
        }
        private void BtnAddGameCover_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Filtra para exibir apenas arquivos de imagem
            openFileDialog.Filter = "Arquivos de Imagem|*.jpg;*.jpeg;*.png;*.gif|Todos os Arquivos|*.*";

            // Executa o diálogo e verifica se o usuário selecionou um arquivo
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Cria um novo BitmapImage
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();

                    // Define a Uri da imagem a partir do caminho do arquivo
                    bitmap.UriSource = new Uri(openFileDialog.FileName);

                    // Finaliza a inicialização da imagem
                    bitmap.EndInit();

                    // Atribui a imagem ao controle Image no XAML
                    ImgGameCoverViewer.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar a imagem: " + ex.Message);
                }
            }
        }        
        private void BtnAddGameScreenshot_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivos de Imagem|*.jpg;*.jpeg;*.png;*.gif|Todos os Arquivos|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ImgGameScreenshotViewer.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar o screenshot: " + ex.Message);
                }
            }
        }
        private void BtnAddRom_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Exemplo: Filtro para arquivos de texto. Você pode ajustar conforme sua necessidade.
            openFileDialog.Filter = "Todos os Arquivos|*.*";

            // Abre o diálogo e verifica se o usuário selecionou um arquivo
            if (openFileDialog.ShowDialog() == true)
            {
                // Atribui o caminho completo do arquivo à TextBox
                RomPathTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
