using SQLGen.ViewModels;

namespace SQLGen.SQLGenerator;

public interface ISQLGenerator
{
	public string Generate(IEnumerable<TableViewModel> tables);
}
