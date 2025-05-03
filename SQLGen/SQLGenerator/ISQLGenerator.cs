using SQLGen.Models;

namespace SQLGen.SQLGenerator;

public interface ISQLGenerator
{
	public string Name { get; }
	public string Generate(IEnumerable<Table> tables);
}
