
using System;

namespace DBComparerLibrary.DBSchema
{
    public class Column : IEquatable<Column>
    {
        public Column(string name, int type, int precision = 0, int scale = 0, int maxSymb = 0)
        {
            Name = name;
            Type = type;
            this.precision = precision;
            this.scale = scale;
            this.maxSymb = maxSymb;
        }

        public string Name { get;}
        public int Type { get; }
        public int precision { get; }
        public int scale { get; }
        public int maxSymb { get; }

        public bool Equals(Column other)
        {
            if (other == null)
                return false;

            return this.Type.Equals(other.Type) &&
                this.precision.Equals(other.precision) &&
                this.scale.Equals(other.scale) &&
                this.maxSymb.Equals(other.maxSymb) &&
                (
                    object.ReferenceEquals(this.Name, other.Name) ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                );
        }
    }
    
}
