namespace BlazorApp.Services;

public interface IToastService
{
    event Action<ToastMessage>? OnShow;

    void ShowSuccess(string message, string? title = null);
    void ShowError(string message, string? title = null);
    void ShowInfo(string message, string? title = null);
    void ShowWarning(string message, string? title = null);
}

public record ToastMessage(string Message, string? Title, ToastType Type);

public enum ToastType
{
    Success,
    Error,
    Info,
    Warning
}

