using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class PrimaryKey : IEquatable<PrimaryKey>
    {
        public string PkName { get; }
        public string Clustered { get; }
        public Dictionary<string, bool> columns;
        

        public PrimaryKey(string pkName, string clustered, Dictionary<string, bool> columns)
        {
            PkName = pkName;
            Clustered = clustered;
            this.columns = columns;
        }

        public bool Equals(PrimaryKey other)
        {
            if (other == null)
                return false;

            return
                    Comparer.DictEquals(this.columns, other.columns) &&
                    Comparer.CompareStrings(this.Clustered, other.Clustered) &&
                    Comparer.CompareStrings(this.PkName, other.PkName);
        }
    }
}
