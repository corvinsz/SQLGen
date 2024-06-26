using CommunityToolkit.Mvvm.ComponentModel;

namespace SQLGen.ViewModels;

public abstract partial class SelectableElement : ObservableObject
{
	[ObservableProperty]
	private bool _isSelected;

	public Type SelfType => this.GetType();
}

public static class SelectableElementExtensions
{
	public static List<TableViewModel> WhereTablesNotConnectedToThis(this IEnumerable<SelectableElement> items, TableViewModel table)
	{
		var allTables = items.OfType<TableViewModel>().ToList();

		foreach (LineViewModel connection in items.OfType<LineViewModel>())
		{
			if (connection.From == table || connection.To == table)
			{
				allTables.Remove(connection.From);
				allTables.Remove(connection.To);
			}
		}

		allTables.Remove(table);

		return allTables;
	}
}
