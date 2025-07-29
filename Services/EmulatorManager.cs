using RetroGamesLauncher.Models.Enums;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace RetroGamesLauncher.Services;

/// <summary>
/// Gerencia a execução dos emuladores, incluindo iniciar e fechar processos de emuladores.
/// </summary>
public static class EmulatorManager
{
    static Process _currentEmulatorProcess;

    /// <summary>
    /// Inicia um emulador com base no ID fornecido e carrega a ROM especificada.
    /// Fecha o emulador anterior, se estiver em execução.
    /// </summary>
    /// <param name="emulatorId">O identificador do emulador (definido no enum <see cref="Emulators"/>).</param>
    /// <param name="romPath">Caminho relativo do arquivo de ROM a ser executado.</param>
    public static void LaunchEmulator(Emulators emulatorId, string romPath)
    {
        var (folderName, exeName, arguments) = GetEmulatorConfig(emulatorId);

        if (string.IsNullOrEmpty(exeName))
        {
            MessageBox.Show($"Erro: Nenhum executável de emulador encontrado para o ID '{emulatorId}'.", "Erro de Emulador", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        string emulatorDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Emulators", folderName);
        string emulatorExePath = Path.Combine(emulatorDirPath, exeName);

        if (!File.Exists(emulatorExePath))
        {
            MessageBox.Show($"Erro: O executável do emulador '{exeName}' não foi encontrado em '{emulatorDirPath}'.", "Erro de Emulador", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            string romFullPath = Path.Combine(AppContext.BaseDirectory, romPath);

            // Fecha o processo anterior antes de iniciar um novo
            CloseEmulator();

            ProcessStartInfo startInfo = new()
            {
                FileName = emulatorExePath,
                Arguments = $"\"{romFullPath}\" {(string.IsNullOrWhiteSpace(arguments) ? "" : arguments)}",
                WorkingDirectory = emulatorDirPath,
                UseShellExecute = true
            };

            _currentEmulatorProcess = Process.Start(startInfo);
            Debug.WriteLine($"Mesen iniciado com: {startInfo.Arguments}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao iniciar o emulador: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Encerra o processo do emulador atualmente em execução, se houver.
    /// </summary>
    public static void CloseEmulator()
    {
        try
        {
            if (_currentEmulatorProcess != null && !_currentEmulatorProcess.HasExited)
            {
                _currentEmulatorProcess.Kill();
                _currentEmulatorProcess.Dispose();
                _currentEmulatorProcess = null;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao tentar fechar o emulador: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Retorna as configurações do emulador com base no ID informado.
    /// Inclui nome da pasta, nome do executável e argumentos de linha de comando.
    /// </summary>
    /// <param name="emulatorId">O identificador do emulador (enum <see cref="Emulators"/>).</param>
    /// <returns>Tupla contendo o nome da pasta, nome do executável e argumentos de linha de comando.</returns>
    private static (string folderName, string ExeName, string Arguments) GetEmulatorConfig(Emulators emulatorId)
    {
        return emulatorId switch
        {
            Emulators.Mesen => ("Mesen", "mesen.exe", "--fullscreen"),
            _ => (null, null, null),
        };
    }
}
