
using System;
using System.Collections.Generic;

namespace DBComparerLibrary.DBSchema
{
    public class Schema : IEquatable<Schema>
    {
        public Schema(string name, int schema_id)
        {
            Name = name;
            Schema_id = schema_id;
            tables = new Dictionary<string, Table>();
            views = new Dictionary<string, View>();
        }

        public string Name { get; }
        public int Schema_id { get; }//не участвует в сравнении
        public Dictionary<string, Table> tables;
        public Dictionary<string, View> views;

        public bool Equals(Schema other)
        {
            if (other == null)
                return false;

            return CollectionComparer.DictEquals(this.tables, other.tables) &&
                CollectionComparer.DictEquals(this.views, other.views) &&
                (
                    object.ReferenceEquals(this.Name, other.Name) ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                );
        }
    }
}
