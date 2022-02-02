
using System.Collections.Generic;

namespace DBComparerLibrary.DBSchema
{
    public class Schema
    {
        public Schema(string name, int schema_id)
        {
            Name = name;
            Schema_id = schema_id;
            tables = new Dictionary<string, Table>();
        }

        public string Name { get; }
        public int Schema_id { get; }
        public Dictionary<string, Table> tables;
    }
}
