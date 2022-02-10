using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class ForeignKey : IEquatable<ForeignKey>
    {

        public string FKName { get; }
        public List<ForeignKeyRefs> refs;

        public ForeignKey(string fKName, List<ForeignKeyRefs> refs)
        {
            FKName = fKName.Trim();
            this.refs = refs;
        }

        public bool Equals(ForeignKey other)
        {
            if (other == null)
                return false;

            return
                    Comparer.EnumEquals(this.refs, other.refs) &&
                    Comparer.CompareStrings(this.FKName, other.FKName);
        }
    }
}
