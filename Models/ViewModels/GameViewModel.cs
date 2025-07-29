using System.Windows.Media.Imaging;

namespace RetroGamesLauncher.Models.ViewModels;
public class GameViewModel
{
    public string Title { get; set; }
    public BitmapImage ImageSource { get; set; }
    public GameInfo Game { get; set; }
}

