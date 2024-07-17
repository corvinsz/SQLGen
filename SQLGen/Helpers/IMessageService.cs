using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.Helpers;

//Provides a way to ShowMessages but also exposes the underlying object so (if needed) more control over the message shown is provided
public interface IMessageService<T>
{
	public T Provider { get; }
	public void ShowMessage(string message);
}

public class SnackbarMessageService : IMessageService<SnackbarMessageQueue>
{
	public SnackbarMessageService(SnackbarMessageQueue provider)
	{
		Provider = provider;
	}

	public SnackbarMessageQueue Provider { get; }

	public void ShowMessage(string message)
	{
		Provider.Enqueue(message);
	}
}
