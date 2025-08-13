using RetroGamesLauncher.Data.Repositories;
using RetroGamesLauncher.Models;
using System.Windows;

namespace RetroGamesLauncher.Views.Shared;

/// <summary>
/// Lógica interna para GameGenderForm.xaml
/// </summary>
public partial class GameGenderForm : Window
{
    private readonly IGameGenderRepository _gameGenderRepository;

    public event EventHandler<EventArgs> GenderAdded;
    public GameGenderForm(IGameGenderRepository gameGenderRepository)
    {
        InitializeComponent();
        _gameGenderRepository = gameGenderRepository;
        GenderNameTextBox.Focus();
    }
            
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(GenderNameTextBox.Text))
        {
            TxtGenderNameFeedBack.Text = "O gênero não pode ser vazio. ⚠️";
            TxtGenderNameFeedBack.Visibility = Visibility.Visible;
            return;
        }            
        _gameGenderRepository.Add(new GameGender(GenderNameTextBox.Text));

        GenderAdded?.Invoke(this, EventArgs.Empty);

        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
