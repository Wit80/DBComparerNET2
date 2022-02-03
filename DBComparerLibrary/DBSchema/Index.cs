
using System;
using System.Collections.Generic;

namespace DBComparerLibrary.DBSchema
{
    public class Index: IEquatable<Index>
    {

        public string indexName { get; }
        public DateTime dtCreate { get; }
        public DateTime dtUpdate { get; }
        public List<string> columns;

        public Index(string indexName, DateTime dtCreate, DateTime dtUpdate, string column)
        {
            this.indexName = indexName;
            this.dtCreate = dtCreate;
            this.dtUpdate = dtUpdate;
            columns = new List<string> { column };
        }

        public bool Equals(Index other)
        {
            if (other == null)
                return false;

            return CollectionComparer.EnumEquals(this.columns, other.columns) &&
                (
                    object.ReferenceEquals(this.indexName, other.indexName) ||
                    this.indexName != null &&
                    this.indexName.Equals(other.indexName)
                );
        }
    }
}
