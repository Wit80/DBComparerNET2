using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class Index : IEquatable<Index>
    {

        public string IndexName { get; }
        public IndexTypeEnum IndexType { get; }
        public bool IsUniq { get; }
        public string Clustered { get; }
        public Dictionary<string,bool> columns;

        public Index(string indexName, int indexType, bool isUniq, Dictionary<string, bool> columns, string clustered)
        {
            IndexName = indexName.Trim();
            IndexType = (IndexTypeEnum)indexType;
            IsUniq = isUniq;
            this.columns = columns;
            Clustered = clustered;
        }

        public bool Equals(Index other)
        {
            if (other == null)
                return false;

            return this.IndexType.Equals(other.IndexType) &&
                this.IsUniq.Equals(other.IsUniq) &&
                Comparer.DictEquals(this.columns, other.columns) &&
                Comparer.CompareStrings(this.IndexName, other.IndexName)&&
                Comparer.CompareStrings(this.Clustered, other.Clustered);
        }
    }
}
