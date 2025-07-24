using RetroGamesLauncher.Views;

namespace RetroGamesLauncher.Services
{
    public static class ToastMessages
    {
        public static void ShowTemporaryNotification(string message)
        {
            var toast = new ToastWindow(message);
            toast.Activate();
            toast.Show();
        }
    }
}
