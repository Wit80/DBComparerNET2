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
        public bool IsPrimary { get; }
        public bool IsDesc { get; }
        public List<string> columns;

        public Index(string indexName, int indexType, bool isUniq, bool isPrimary, bool isDesc, List<string> columns)
        {
            IndexName = indexName.Trim();
            IndexType = (IndexTypeEnum)indexType;
            IsUniq = isUniq;
            IsPrimary = isPrimary;
            IsDesc = isDesc;
            this.columns = columns;
        }

        public bool Equals(Index other)
        {
            if (other == null)
                return false;

            return this.IndexType.Equals(other.IndexType) &&
                this.IsUniq.Equals(other.IsUniq) &&
                this.IsPrimary.Equals(other.IsPrimary) &&
                this.IsDesc.Equals(other.IsDesc) &&
                Comparer.EnumEquals(this.columns, other.columns) &&
                Comparer.CompareStrings(this.IndexName, other.IndexName);
        }
    }
}
