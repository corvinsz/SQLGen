using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen;

public enum DBMS
{
    MySQL,
    MariaDB,
    MSSQLServer
}

public class MSSQLServerGenerator
{
    public string Generate(IEnumerable<TableViewModel> tables, DBMS dbms)
    {
        StringBuilder sql = new();

        foreach (TableViewModel table in tables)
        {
            sql.AppendLine(table.GenerateSQL(dbms));
        }
    }
}
