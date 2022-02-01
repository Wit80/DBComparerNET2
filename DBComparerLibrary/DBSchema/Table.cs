using System.Collections.Generic;

namespace DBComparerLibrary.DBSchema
{
    public class Table
    {
        IEnumerable<Column> columns;
        IEnumerable<Index> indexes;
    }
}
