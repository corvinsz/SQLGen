using CommunityToolkit.Mvvm.ComponentModel;

namespace SQLGen.Models;

public abstract partial class SelectableElement : ObservableObject
{
	[ObservableProperty]
	private bool _isSelected;

	public Type SelfType => this.GetType();
}

public static class SelectableElementExtensions
{
	public static List<Table> WhereTablesNotConnectedToThis(this IEnumerable<SelectableElement> items, Table table)
	{
		var allTables = items.OfType<Table>().ToList();

		foreach (Line connection in items.OfType<Line>())
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
