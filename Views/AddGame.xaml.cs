using RetroGamesLauncher.Data;
using RetroGamesLauncher.Models;
using System.Windows;

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
                ImagePath = ImagePathTextBox.Text,
                ScreenshotPath = ScreenshotPathTextBox.Text,
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
    }
}
