using System;
using System.Collections.Generic;

namespace DBComparerLibrary.DBSchema
{
    public class Table : IEquatable<Table>
    {
        public Table(ulong objectId, string name, int rowCount, DateTime Create, DateTime Update, Dictionary<string, Column> columns, Dictionary<string, Index> indexes)
        {
            this.objectId = objectId;
            Name = name;
            this.rowCount = rowCount;
            this.columns = columns;
            this.indexes = indexes;
            dtCreate = Create;
            dtUpdate = Update;
            constraints = new List<Constraints>();
        }

        UInt64 objectId { get; }
        public string Name { get; }
        public int rowCount { get; }
        public DateTime dtCreate { get; }
        public DateTime dtUpdate { get; }
        public Dictionary<string,Column> columns { get;}
        public Dictionary<string,Index> indexes { get; }
        public List<Constraints> constraints { get; set; }

        public bool Equals(Table other)
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
                Comparers.DictEquals(this.columns, other.columns)
                &&
                Comparers.DictEquals(this.indexes, other.indexes)
                &&
                Comparers.EnumEquals(this.constraints, other.constraints);
        }
    }
}
