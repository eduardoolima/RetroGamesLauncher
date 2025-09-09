using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using RetroGamesLauncher.Models.AuxModels;
using RetroGamesLauncher.Services;
using RetroGamesLauncher.Utils.FormsValidation;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using System.Windows.Controls;

namespace RetroGamesLauncher.Views.UserControls.SettingsTabs;

/// <summary>
/// Interação lógica para SettingsFilesPath.xam
/// </summary>
public partial class SettingsFilesPath : UserControl
{
    public SettingsFilesPath()
    {
        InitializeComponent();
        SettingsFilesPathLoad();
    }

    private void SettingsFilesPathLoad()
    {
        string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\appsettings.json");
        if (!File.Exists(configFilePath))
            return;
        string jsonContent = File.ReadAllText(configFilePath);
        JObject jsonObject = JObject.Parse(jsonContent);
        JObject filePathsObject = (JObject)jsonObject["FilePaths"];
        if (filePathsObject != null)
        {
            if (filePathsObject.ContainsKey("Roms"))
            {
                RomsPathTextBox.Text = filePathsObject["Roms"].ToString();
            }
            if (filePathsObject.ContainsKey("Images"))
            {
                CoversPathTextBox.Text = filePathsObject["Images"].ToString();
            }
            if (filePathsObject.ContainsKey("Emulators"))
            {
                ScreenshotsPathTextBox.Text = filePathsObject["Emulators"].ToString();
            }
        }
    }

    private void BtnChooseRomFolder_Click(object sender, RoutedEventArgs e)
    {
        using (var dialog = new CommonOpenFileDialog())
        {
            dialog.IsFolderPicker = true; // Define que a caixa de diálogo é para selecionar pastas
            dialog.Title = "Selecione a pasta de Roms";

            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                RomsPathTextBox.Text = dialog.FileName;
            }
        }
    }

    private void BtnChooseCoverFolder_Click(object sender, RoutedEventArgs e)
    {
        using (var dialog = new CommonOpenFileDialog())
        {
            dialog.IsFolderPicker = true;
            dialog.Title = "Selecione a pasta de Capas";

            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                CoversPathTextBox.Text = dialog.FileName;
            }
        }
    }

    private void BtnScreenshotsFolder_Click(object sender, RoutedEventArgs e)
    {
        using (var dialog = new CommonOpenFileDialog())
        {
            dialog.IsFolderPicker = true;
            dialog.Title = "Selecione a pasta de Screenshots";

            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                ScreenshotsPathTextBox.Text = dialog.FileName;
            }
        }
    }

    private void SavePathsButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (SettingsFilesPathValidation.FormValidate(this))
            {
                string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\appsettings.json");
                if (!File.Exists(configFilePath))
                    return;

                string jsonContent = File.ReadAllText(configFilePath);
                JObject jsonObject = JObject.Parse(jsonContent);
                JObject filePathsObject = (JObject)jsonObject["FilePaths"];
                if (filePathsObject != null)
                {
                    if (jsonObject.ContainsKey("Roms"))
                    {
                        jsonObject["Roms"] = RomsPathTextBox.Text;
                    }
                    if (jsonObject.ContainsKey("Images"))
                    {
                        jsonObject["Images"] = CoversPathTextBox.Text;
                    }
                    if (jsonObject.ContainsKey("Emulators"))
                    {
                        jsonObject["Emulators"] = ScreenshotsPathTextBox.Text;
                    } 
                }
                string updatedJson = jsonObject.ToString(Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(configFilePath, updatedJson);

                //implementar ~logica para atualizar os caminhos dos jogos no banco de dados e mover os arquivos existentes

                ToastMessages.ShowTemporaryNotification("✔️ Caminhos alterados com sucesso!", TypeToastMessage.Success);
            }
        }
        catch (Exception)
        {
            ToastMessages.ShowTemporaryNotification("☠ Deu Ruim!", TypeToastMessage.Error);
        }
    }
}    
