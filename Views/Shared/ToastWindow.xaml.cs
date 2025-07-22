using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace RetroGamesLauncher.Views
{
    /// <summary>
    /// Lógica interna para ToastWindow.xaml
    /// </summary>
    public partial class ToastWindow : Window
    {
        private DispatcherTimer _timer;
        public ToastWindow(string message, int durationSeconds = 3)
        {
            InitializeComponent();
            MessageText.Text = message;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(durationSeconds)
            };
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            FadeOut();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Posicionar janela centralizada horizontal, 50px do topo da tela principal
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            Left = (screenWidth - Width) / 2;
            Top = 50;

            FadeIn();

            _timer.Start();
        }

        private void FadeIn()
        {
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500));
            (Content as UIElement).BeginAnimation(OpacityProperty, fadeIn);
        }

        private void FadeOut()
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));
            fadeOut.Completed += (s, e) => Close();
            (Content as UIElement).BeginAnimation(OpacityProperty, fadeOut);
        }
    }
}
