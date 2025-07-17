using MaterialDesignThemes.Wpf;

namespace SQLGen.Services;

public interface IDialogService
{
	Task<object?> Show(object content);
	Task<object?> Show(object content, string dialogIdentifier);
}

public class DialogService : IDialogService
{
	public async Task<object?> Show(object content)
		=> await DialogHost.Show(content);

	public async Task<object?> Show(object content, string dialogIdentifier)
		=> await DialogHost.Show(content, dialogIdentifier);
}
