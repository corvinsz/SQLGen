using MaterialDesignThemes.Wpf;

namespace SQLGen.Services;
public interface IDialogService
{
	Task<object?> ShowMessageDialog(object content);
}
internal class DialogService : IDialogService
{
	public async Task<object?> ShowMessageDialog(object content)
	{
		return await DialogHost.Show(content);
	}
}
