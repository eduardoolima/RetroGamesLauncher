using System.IO;

namespace RetroGamesLauncher.Utils;

/// <summary>
/// Fornece métodos de extensão para validar caminhos de arquivo e diretório.
/// </summary>
public static class PathValidation
{
    /// <summary>
    /// Verifica se o caminho fornecido é válido e se o diretório existe.
    /// </summary>
    /// <remarks>
    /// O método retorna <c>false</c> se o caminho for vazio, não for totalmente qualificado, 
    /// contiver caracteres inválidos ou se o diretório não existir.
    /// </remarks>
    /// <param name="path">O caminho do diretório a ser validado.</param>
    /// <returns><c>true</c> se o caminho for válido e o diretório existir; caso contrário, <c>false</c>.</returns>
    public static bool IsPathValid(string path)
    {
        if (path == string.Empty || Path.IsPathFullyQualified(path) || HasInvalidChars(path) || !Directory.Exists(path))
            return false;
        return true;
    }
    static bool HasInvalidChars(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }
        char[] invalidChars = Path.GetInvalidPathChars();
        return path.IndexOfAny(invalidChars) >= 0;
    }
}
