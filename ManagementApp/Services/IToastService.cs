namespace ManagementApp.Services
{
    public enum ToastLevel
    {
        Info,
        Success,
        Warning,
        Error,
    }
    public interface IToastService : IDisposable
    {
        public event Action<string, ToastLevel>? OnShow;
        public event Action? OnHide;
        public void ShowToast(string message, ToastLevel level);
    }
}
