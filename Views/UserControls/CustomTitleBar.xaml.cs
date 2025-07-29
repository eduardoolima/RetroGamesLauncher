using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RetroGamesLauncher.Views.UserControls;

/// <summary>
/// Interação lógica para CustomTitleBar.xam
/// </summary>
public partial class CustomTitleBar : UserControl
{
    public CustomTitleBar()
    {
        InitializeComponent();
    }

    private Window GetParentWindow() => Window.GetWindow(this);

    private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            GetParentWindow()?.DragMove();
    }

    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        GetParentWindow().WindowState = WindowState.Minimized;
    }

    private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
    {
        var win = GetParentWindow();
        win.WindowState = win.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        GetParentWindow()?.Close();
    }

    private void Close_MouseEnter(object sender, MouseEventArgs e)
    {
        (sender as Button).Background = Brushes.Red;
    }

    private void Close_MouseLeave(object sender, MouseEventArgs e)
    {
        (sender as Button).Background = Brushes.Transparent;
    }
}

