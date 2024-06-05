namespace PCGuy.Models.ViewModels;

public class ErrorViewModel
{
    public string RequestId { get; init; } = default!;

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}