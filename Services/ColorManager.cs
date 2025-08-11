using RetroGamesLauncher.Models.AuxModels;
using System.Windows.Media;

namespace RetroGamesLauncher.Services;

public static class ColorManager
{
    public static SolidColorBrush GetColorBrush(TypeToastMessage color)
    {
        switch (color)
        {
            case TypeToastMessage.Default:
                return new SolidColorBrush(Color.FromArgb(221, 51, 51, 51));
            case TypeToastMessage.Success:
                return new SolidColorBrush(Colors.Green);
            case TypeToastMessage.Error:
                return new SolidColorBrush(Color.FromArgb(255, 173, 47, 47));
            case TypeToastMessage.Warning:
                return new SolidColorBrush(Colors.Yellow);
            case TypeToastMessage.Info:
                return new SolidColorBrush(Colors.Blue);
            default:
                return new SolidColorBrush(Color.FromArgb(221, 51, 51, 51));
        }
    }

    public static SolidColorBrush GetTextColorBrush(TypeToastMessage color)
    {
        switch (color)
        {
            case TypeToastMessage.Default:
                return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            case TypeToastMessage.Success:
                return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            case TypeToastMessage.Error:
                return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            case TypeToastMessage.Warning:
                return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            case TypeToastMessage.Info:
                return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            default:
                return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }
    }
}
