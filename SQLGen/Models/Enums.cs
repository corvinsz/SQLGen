using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.Models;

public enum RelativePosition
{
    //TopLeft,
    Top,
    //TopRight,
    Right,
    //BottomRight,
    Bottom,
    //BottomLeft,
    Left
}

public enum DBMS
{
    MSSQLServer,
    MySQL,
    MariaDB,
    PostgreSQL
}
