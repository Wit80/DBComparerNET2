using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class ForeignKey : IEquatable<ForeignKey>
    {

        public string FKName { get; }
        public string fkTableName { get; }
        public string prTableName { get; }
        public List<string> fkColumnName { get; }
        public List<string> pkColumnName { get; }
        public string deleteRef { get; }
        public string updateRef { get; }

        public ForeignKey(string fKName, string fkTN, string prTN, List<string> fkCN, List<string> pkCN, string delRef, string updRef)
        {
            FKName = fKName.Trim();
            fkTableName = fkTN;
            prTableName = prTN;
            fkColumnName = fkCN;
            pkColumnName = pkCN;
            deleteRef = delRef;
            updateRef = updRef;
            
        }

        public bool Equals(ForeignKey other)
        {
            if (other == null)
                return false;

            return
                    Comparer.EnumEquals(this.fkColumnName, other.fkColumnName) &&
                    Comparer.EnumEquals(this.pkColumnName, other.pkColumnName) &&
                    Comparer.CompareStrings(this.fkTableName, other.fkTableName) &&
                    Comparer.CompareStrings(this.prTableName, other.prTableName) &&
                    Comparer.CompareStrings(this.deleteRef, other.deleteRef) &&
                    Comparer.CompareStrings(this.updateRef, other.updateRef) &&
                    Comparer.CompareStrings(this.FKName, other.FKName);
        }
    }
}
