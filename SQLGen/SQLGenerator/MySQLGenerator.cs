using SQLGen.ViewModels;

namespace SQLGen.SQLGenerator;

internal class MySQLGenerator : ISQLGenerator
{
	public string Name => "MySQL";

	public string Generate(IEnumerable<TableViewModel> tables)
	{
		return "throw new NotImplementedException();";
	}
}
