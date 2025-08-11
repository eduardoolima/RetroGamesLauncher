using RetroGamesLauncher.Models.AuxModels;
using RetroGamesLauncher.Services;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
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

        /// <summary>
        /// Cria e exibe uma janela de toast (notificação pop-up) na tela.
        /// </summary>
        /// <param name="message">A mensagem de texto a ser exibida na notificação.</param>
        /// <param name="typeToastMessage">O tipo da notificação, que define a cor de fundo (padrão, sucesso, erro, etc.).</param>
        /// <param name="durationSeconds">A duração em segundos que a notificação permanecerá visível antes de fechar automaticamente.</param>
        public ToastWindow(string message, TypeToastMessage typeToastMessage = TypeToastMessage.Default, int durationSeconds = 3)
        {
            InitializeComponent();
            MessageText.Text = message;
            MessageText.Foreground = ColorManager.GetTextColorBrush(typeToastMessage);
            ToasMessage.Background = ColorManager.GetColorBrush(typeToastMessage);

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
