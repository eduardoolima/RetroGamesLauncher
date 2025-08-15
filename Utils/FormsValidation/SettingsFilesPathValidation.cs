using RetroGamesLauncher.Views.UserControls.SettingsTabs;
using System.Windows;
using System.Windows.Controls;

namespace RetroGamesLauncher.Utils.FormsValidation;

class SettingsFilesPathValidation
{
    public static bool FormValidate(UserControl uc)
    {
        bool isValid = false;
        if (uc is SettingsFilesPath settingsFilesPath)
        {
            isValid = true;
            settingsFilesPath.TxtRomsFeedBack.Visibility = Visibility.Collapsed;
            settingsFilesPath.TxtCoversFeedBack.Visibility = Visibility.Collapsed;
            settingsFilesPath.TxtScreenshotsFeedBack.Visibility = Visibility.Collapsed;

            if (!PathValidation.IsPathValid(settingsFilesPath.RomsPathTextBox.Text))
            {
                settingsFilesPath.TxtRomsFeedBack.Text = "Caminho para Roms inválido⚠️";
                settingsFilesPath.TxtRomsFeedBack.Visibility = Visibility.Visible;
                settingsFilesPath.RomsPathTextBox.Focus();
                isValid = false;
            }
            if (!PathValidation.IsPathValid(settingsFilesPath.CoversPathTextBox.Text))
            {
                settingsFilesPath.TxtCoversFeedBack.Text = "Caminho para Capas inválido⚠️";
                settingsFilesPath.TxtCoversFeedBack.Visibility = Visibility.Visible;
                settingsFilesPath.CoversPathTextBox.Focus();
                isValid = false;
            }
            if (!PathValidation.IsPathValid(settingsFilesPath.ScreenshotsPathTextBox.Text))
            {
                settingsFilesPath.TxtScreenshotsFeedBack.Text = "Caminho para Screenshots inválido⚠️";
                settingsFilesPath.TxtScreenshotsFeedBack.Visibility = Visibility.Visible;
                settingsFilesPath.ScreenshotsPathTextBox.Focus();
                isValid = false;
            }
        }
        return isValid;
    }      
}
