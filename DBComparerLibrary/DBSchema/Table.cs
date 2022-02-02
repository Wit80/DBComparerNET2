using System.Collections.Generic;

namespace DBComparerLibrary.DBSchema
{
    public class Table
    {
        public string Name { get; set; }
        public int rowCount { get; set; }
        public Dictionary<string,Column> columns { get; set; }
        public Dictionary<string,Index> indexes { get; set; }
        public Dictionary<string, Constraints> constraints { get; set; }

    }
}
