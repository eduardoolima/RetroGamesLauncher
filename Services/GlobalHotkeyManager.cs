using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace RetroGamesLauncher.Services;
public class GlobalHotkeyManager : IDisposable
{
    private readonly IntPtr _windowHandle;
    private HwndSource _source;
    private const int WM_HOTKEY = 0x0312;
    private const int HOTKEY_ID = 1; // Pode ser qualquer número único

    // Modificadores
    private const uint MOD_SHIFT = 0x0004;

    // Tecla VK_ESCAPE
    private const uint VK_ESCAPE = 0x1B;

    public event EventHandler HotkeyPressed;

    public GlobalHotkeyManager(IntPtr windowHandle)
    {
        _windowHandle = windowHandle;
        _source = HwndSource.FromHwnd(_windowHandle);
        if (_source == null)
            throw new Exception("Falha ao obter HwndSource da janela");

        _source.AddHook(HwndHook);

        RegisterHotKey();
    }

    private void RegisterHotKey()
    {
        if (!NativeMethods.RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_SHIFT, VK_ESCAPE))
            throw new InvalidOperationException("Não foi possível registrar o atalho global.");
    }

    private void UnregisterHotKey()
    {
        NativeMethods.UnregisterHotKey(_windowHandle, HOTKEY_ID);
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_HOTKEY)
        {
            if (wParam.ToInt32() == HOTKEY_ID)
            {
                HotkeyPressed?.Invoke(this, EventArgs.Empty);
                handled = true;
            }
        }
        return IntPtr.Zero;
    }

    public void Dispose()
    {
        UnregisterHotKey();
        _source.RemoveHook(HwndHook);
    }

    private static class NativeMethods
    {
        // Registra um atalho global
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        // Remove o atalho
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
