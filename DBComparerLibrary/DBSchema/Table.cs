using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class Table
    {
        IEnumerable<Column> columns;
        IEnumerable<Index> indexes;
    }
}
