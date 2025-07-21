using RetroGamesLauncher.Models.Enums;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace RetroGamesLauncher.Services
{
    public static class EmulatorManager
    {
        /// <summary>
        /// Inicia um emulador, extraindo-o e seus arquivos de configuração para um local temporário.
        /// </summary>
        /// <param name="emulatorId">O ID do emulador a ser lançado.</param>
        /// <param name="romPath">O caminho do arquivo de ROM a ser carregado.</param>
        public static void LaunchEmulator(Emulators emulatorId, string romPath)
        {
            var emulatorConfig = GetEmulatorConfig(emulatorId);
            string folderName = emulatorConfig.folderName;
            string emulatorExeName = emulatorConfig.ExeName;
            string additionalArguments = emulatorConfig.Arguments;

            if (string.IsNullOrEmpty(emulatorExeName))
            {
                MessageBox.Show($"Erro: Nenhum executável de emulador encontrado para o ID '{emulatorId}'.", "Erro de Emulador", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string emulatorDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Emulators", folderName);
            string emulatorExePath = Path.Combine(emulatorDirPath, emulatorExeName);

            if (!File.Exists(emulatorExePath))
            {
                MessageBox.Show($"Erro: O executável do emulador '{emulatorExeName}' não foi encontrado em '{emulatorDirPath}'.", "Erro de Emulador", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string romFullPath = Path.Combine(AppContext.BaseDirectory, romPath);
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = emulatorExePath,
                    Arguments = $"\"{romFullPath}\" {(string.IsNullOrWhiteSpace(additionalArguments) ? "" : additionalArguments)}",
                    WorkingDirectory = emulatorDirPath,
                    UseShellExecute = true
                };

                Process.Start(startInfo);
                Debug.WriteLine($"Mesen iniciado com: {startInfo.Arguments}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao iniciar o emulador: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private static (string folderName, string ExeName, string Arguments) GetEmulatorConfig(Emulators emulatorId)
        {

            return emulatorId switch
            {
                Emulators.Mesen => ("Mesen", "mesen.exe", "--fullscreen"),
                _ => (null, null, null),
            };
        }
   
    }
}
