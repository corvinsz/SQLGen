using SQLGen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.SQLGenerator;

internal class MySQLGenerator : ISQLGenerator
{
	public string Name => "MySQL";

	public string Generate(IEnumerable<Table> tables)
	{
		throw new NotImplementedException();
	}
}
