
using System;

namespace DBComparerLibrary.DBSchema
{
    public class Column : IEquatable<Column>
    {
        public Column(string name, int type, string typeName, int precision = 0, int scale = 0, int maxSymb = 0, bool isNullable = true)
        {
            Name = name;
            Type = type;
            this.precision = precision;
            this.scale = scale;
            this.maxSymb = maxSymb;
            this.isNullable = isNullable;
            this.TypeName = typeName;
        }

        public string Name { get;}
        public int Type { get; }
        public string TypeName { get; }
        public int precision { get; }
        public int scale { get; }
        public int maxSymb { get; }
        public bool isNullable { get; }

        public bool Equals(Column other)
        {
            if (other == null)
                return false;

            return this.isNullable.Equals(other.isNullable) &&
                this.precision.Equals(other.precision) &&
                this.scale.Equals(other.scale) &&
                this.maxSymb.Equals(other.maxSymb) &&
                (
                    object.ReferenceEquals(this.Name, other.Name) ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) &&
                (
                    object.ReferenceEquals(this.TypeName, other.TypeName) ||
                    this.TypeName != null &&
                    this.TypeName.Equals(other.TypeName)
                );
        }
    }
    
}
