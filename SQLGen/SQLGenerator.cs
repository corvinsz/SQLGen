using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen;

public enum DBMS
{
    MSSQLServer,
    MySQL,
    MariaDB,
    PostgreSQL
}

public class SQLGenerator
{
    public static string Generate(IEnumerable<TableViewModel> tables, DBMS dbms)
    {
        StringBuilder sql = new();
        foreach (TableViewModel table in tables)
        {
            sql.AppendLine(table.GenerateSQL(dbms));
            sql.AppendLine();
        }
        return sql.ToString();
    }
}
