using MaterialDesignThemes.Wpf;

namespace SQLGen.Services;

public record ErrorModel(string? Title, string? Message);
public interface IErrorHandler
{
	public void HandleError(Exception exception);
}

public class ErrorHandler : IErrorHandler
{
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;
	private readonly IDialogService _dialogService;

	public ErrorHandler(ISnackbarMessageQueue snackbarMessageQueue, IDialogService dialogService)
	{
		_snackbarMessageQueue = snackbarMessageQueue;
		_dialogService = dialogService;
	}

	public void HandleError(Exception exception)
	{
		_snackbarMessageQueue.Enqueue($"Error: {exception.Message}", "Details", () =>
		{
			var errorModel = new ErrorModel("Error", exception.ToString());
			_dialogService.Show(new Views.Dialogs.ErrorHandlerDialog(errorModel));
		});
	}
}
