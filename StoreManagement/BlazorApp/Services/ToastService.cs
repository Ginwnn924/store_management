namespace BlazorApp.Services;

public class ToastService : IToastService
{
    public event Action<ToastMessage>? OnShow;

    public void ShowSuccess(string message, string? title = null)
    {
        OnShow?.Invoke(new ToastMessage(message, title ?? "Thành công", ToastType.Success));
    }

    public void ShowError(string message, string? title = null)
    {
        OnShow?.Invoke(new ToastMessage(message, title ?? "Lỗi", ToastType.Error));
    }

    public void ShowInfo(string message, string? title = null)
    {
        OnShow?.Invoke(new ToastMessage(message, title ?? "Thông báo", ToastType.Info));
    }

    public void ShowWarning(string message, string? title = null)
    {
        OnShow?.Invoke(new ToastMessage(message, title ?? "Cảnh báo", ToastType.Warning));
    }
}

