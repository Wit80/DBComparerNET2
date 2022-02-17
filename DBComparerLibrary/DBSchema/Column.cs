using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    
    public class Column : IEquatable<Column>
    {
        public Column(string columnName, string typeName, int maxLength, int precision, int scale, int maxSymb,
            bool isNullable, string definition = "", string DF_name = "", string collation = "", int seed = 0, int increment = 0, string def = "")
        {
            ColumnName = columnName.Trim();
            TypeName = typeName.Trim();
            MaxLength = maxLength;
            Precision = precision;
            Scale = scale;
            MaxSymb = maxSymb;
            IsNullable = isNullable;
            DefaultVal = definition.Trim();
            ConstraintName = DF_name.Trim();
            CollationName = collation.Trim();
            SeedValue = seed;
            IncrementValue = increment;
            Definition = def;
        }

        public string ColumnName { get; }
        public string TypeName { get; }
        public int MaxLength { get; }
        public int Precision { get; }
        public int Scale { get; }
        public int MaxSymb { get; }
        public bool IsNullable { get; }
        public string DefaultVal { get; }
        public string ConstraintName { get; }
        public string CollationName { get; }
        public int SeedValue { get; }
        public int IncrementValue { get; }
        public string Definition { get; }

        public bool Equals(Column other)
        {
            if (other == null)
                return false;

            return this.MaxLength.Equals(other.MaxLength) &&
                this.Precision.Equals(other.Precision) &&
                this.Scale.Equals(other.Scale) &&
                this.MaxSymb.Equals(other.MaxSymb) &&
                this.IsNullable.Equals(other.IsNullable) &&
                this.SeedValue.Equals(other.SeedValue) &&
                this.IncrementValue.Equals(other.IncrementValue) &&
                Comparer.CompareStrings(this.ColumnName, other.ColumnName) &&
                Comparer.CompareStrings(this.DefaultVal, other.DefaultVal) &&
                Comparer.CompareStrings(this.ConstraintName, other.ConstraintName) &&
                Comparer.CompareStrings(this.CollationName, other.CollationName) &&
                Comparer.CompareStrings(this.Definition, other.Definition) &&
                Comparer.CompareStrings(this.TypeName, other.TypeName);
        }
    }
}
