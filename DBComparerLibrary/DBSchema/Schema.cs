
using System.Collections.Generic;

namespace DBComparerLibrary.DBSchema
{
    public class Schema
    {
        public Schema(string name, int schema_id)
        {
            Name = name;
            Schema_id = schema_id; 
        }

        public string Name { get; }
        public int Schema_id { get; }
        public IEnumerable<Table> tables { get; }
    }
}
