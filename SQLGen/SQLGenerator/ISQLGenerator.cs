using SQLGen.ViewModels;

namespace SQLGen.SQLGenerator;

public interface ISQLGenerator
{
	public string Name { get; }
	public string Generate(IEnumerable<TableViewModel> tables);
}
