using System;

namespace DBComparerLibrary.DBSchema
{
    public class Constraints:IEquatable<Constraints>
    {
        public Constraints(string name, DateTime dtCreate, DateTime dtUpdate, ConstraintsTypeEnum type)
        {
            Name = name;
            this.dtCreate = dtCreate;
            this.dtUpdate = dtUpdate;
            Type = type;
        }

        public string Name { get; }
        public DateTime dtCreate { get; }
        public DateTime dtUpdate { get; }
        public ConstraintsTypeEnum Type { get; }

        public bool Equals(Constraints other)
        {
            if (other == null)
                return false;

            return this.Type.Equals(other.Type) &&
                (
                    object.ReferenceEquals(this.Name, other.Name) ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                );
        }
    }
}
