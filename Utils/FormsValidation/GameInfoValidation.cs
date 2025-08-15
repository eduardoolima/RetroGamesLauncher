using RetroGamesLauncher.Views;
using System.Windows;

namespace RetroGamesLauncher.Utils.FormsValidation;

public class GameInfoValidation
{
    public static bool FormValidate(Window window)
    {
        bool isValid = false;
        if (window is GameForm addEditGame)
        {
            isValid = true;
            addEditGame.TxtTitleFeedBack.Visibility = Visibility.Collapsed;
            addEditGame.TxtRomPathFeedBack.Visibility = Visibility.Collapsed;
            addEditGame.TxtEmulatorFeedBack.Visibility = Visibility.Collapsed;
            if (addEditGame.TitleTextBox.Text == string.Empty)
            {
                addEditGame.TxtTitleFeedBack.Text = "Defina um Título ⚠️";
                addEditGame.TxtTitleFeedBack.Visibility = Visibility.Visible;
                addEditGame.TitleTextBox.Focus();
                isValid = false;
            }            
            if (addEditGame.EmulatorComboBox.SelectedIndex < 0)
            {
                addEditGame.TxtEmulatorFeedBack.Text = "Selecione um Emulador ⚠️";
                addEditGame.TxtEmulatorFeedBack.Visibility = Visibility.Visible;
                addEditGame.EmulatorComboBox.Focus();
                isValid = false;
            }
            if (!PathValidation.IsPathValid(addEditGame.RomPathTextBox.Text))
            {
                addEditGame.TxtRomPathFeedBack.Text = "Caminho da Rom inválido ⚠️";
                addEditGame.TxtRomPathFeedBack.Visibility = Visibility.Visible;
                addEditGame.RomPathTextBox.Focus();
                isValid = false;
            }
        }        
        return isValid;
    }

}
