using System;
using System.Collections.Generic;

namespace DBComparerLibrary.DBSchema
{
    public class ddTable : IEquatable<ddTable>
    {
        public ddTable(ulong objectId, string name, int rowCount, DateTime Create, DateTime Update, Dictionary<string, ddColumn> columns, Dictionary<string, ddIndex> indexes)
        {
            this.objectId = objectId;
            Name = name;
            this.rowCount = rowCount;
            this.columns = columns;
            this.indexes = indexes;
            dtCreate = Create;
            dtUpdate = Update;
            constraints = new Dictionary<string, ddConstraints>();
        }

        UInt64 objectId { get; }
        public string Name { get; }
        public int rowCount { get; }
        public DateTime dtCreate { get; }
        public DateTime dtUpdate { get; }
        public Dictionary<string,ddColumn> columns { get;}
        public Dictionary<string,ddIndex> indexes { get; }
        public Dictionary<string,ddConstraints> constraints { get; set; }

        public bool Equals(ddTable other)
        {
            if (other == null)
                return false;

            return this.objectId.Equals(other.objectId) &&
                this.rowCount.Equals(other.rowCount) &&
                (
                    object.ReferenceEquals(this.Name, other.Name) ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                )
                &&
                Comparer.DictEquals(this.columns, other.columns)
                &&
                Comparer.DictEquals(this.indexes, other.indexes)
                &&
                Comparer.DictEquals(this.constraints, other.constraints);
        }
    }
}
