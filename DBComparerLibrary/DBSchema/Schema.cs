using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
   
    public class Schema : IEquatable<Schema>
    {
        public Schema(string sch_Name, string sch_Owner)
        {
            SchemaName = sch_Name.Trim();
            Owner = sch_Owner.Trim();
        }

        public string SchemaName { get; }
        public string Owner { get; }

        public bool Equals(Schema other)
        {
            if (other == null)
                return false;

            return Comparer.CompareStrings(this.Owner, other.Owner) &&
                Comparer.CompareStrings(this.SchemaName, other.SchemaName);
        }
    }
}
