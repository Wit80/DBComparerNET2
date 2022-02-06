
using System;

namespace DBComparerLibrary.DBSchema
{
    public class View : IEquatable<View>
    {
        public View(ulong objId, string name, DateTime Create, DateTime Update)
        {
            this.objectId = objId;
            Name = name;
            dtCreate = Create;
            dtUpdate = Update;
        }

        UInt64 objectId { get; }
        public string Name { get; }
        public DateTime dtCreate { get; }
        public DateTime dtUpdate { get; }

        public bool Equals(View other)
        {
            if (other == null)
                return false;

            return (
                    object.ReferenceEquals(this.Name, other.Name) ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                );
        }
    }
}
