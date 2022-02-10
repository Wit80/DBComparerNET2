using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class ForeignKeyRefs : IEquatable<ForeignKeyRefs>
    {
        public string fkTableName { get; }
        public string prTableName { get; }
        public string fkColumnName { get; }
        public string pkColumnName { get; }
        public string deleteRef { get; }
        public string updateRef { get; }

        public ForeignKeyRefs(string fkTableName, string prTableName, string fkColumnName, string pkColumnName, string deleteRef, string updateRef)
        {
            this.fkTableName = fkTableName.Trim();
            this.prTableName = prTableName.Trim();
            this.fkColumnName = fkColumnName.Trim();
            this.pkColumnName = pkColumnName.Trim();
            this.deleteRef = deleteRef.Trim();
            this.updateRef = updateRef.Trim();
        }

        public bool Equals(ForeignKeyRefs other)
        {
            if (other == null)
                return false;

            return 
                    Comparer.CompareStrings(this.fkTableName, other.fkTableName) &&
                    Comparer.CompareStrings(this.prTableName, other.prTableName) &&
                    Comparer.CompareStrings(this.fkColumnName, other.fkColumnName) &&
                    Comparer.CompareStrings(this.pkColumnName, other.pkColumnName) &&
                    Comparer.CompareStrings(this.deleteRef, other.deleteRef) &&
                    Comparer.CompareStrings(this.updateRef, other.updateRef);
        }
    }
}
