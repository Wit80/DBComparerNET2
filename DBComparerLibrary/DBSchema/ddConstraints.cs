using System;

namespace DBComparerLibrary.DBSchema
{
    public class ddConstraints:IEquatable<ddConstraints>
    {
        public ddConstraints(string name, DateTime dtCreate, DateTime dtUpdate, ddConstraintsTypeEnum type)
        {
            Name = name;
            this.dtCreate = dtCreate;
            this.dtUpdate = dtUpdate;
            Type = type;
        }

        public string Name { get; }
        public DateTime dtCreate { get; }
        public DateTime dtUpdate { get; }
        public ddConstraintsTypeEnum Type { get; }

        public bool Equals(ddConstraints other)
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
