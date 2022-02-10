
using System;
using System.Collections.Generic;

namespace DBComparerLibrary.DBSchema
{
    public class ddSchema : IEquatable<ddSchema>
    {
        public ddSchema(string name, int schema_id)
        {
            Name = name;
            Schema_id = schema_id;
            tables = new Dictionary<string, ddTable>();
            views = new Dictionary<string, ddView>();
        }

        public string Name { get; }
        public int Schema_id { get; }//не участвует в сравнении
        public Dictionary<string, ddTable> tables;
        public Dictionary<string, ddView> views;

        public bool Equals(ddSchema other)
        {
            if (other == null)
                return false;

            return Comparer.DictEquals(this.tables, other.tables) &&
                Comparer.DictEquals(this.views, other.views) &&
                (
                    object.ReferenceEquals(this.Name, other.Name) ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                );
        }
    }
}
