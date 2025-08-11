using RetroGamesLauncher.Models.AuxModels;
using RetroGamesLauncher.Views;

namespace RetroGamesLauncher.Services;
public static class ToastMessages
{
    public static void ShowTemporaryNotification(string message, TypeToastMessage typeToastMessage = TypeToastMessage.Default, int durationInSeconds = 3)
    {
        var toast = new ToastWindow(message, typeToastMessage, durationInSeconds);
        toast.Activate();
        toast.Show();
    }
}
