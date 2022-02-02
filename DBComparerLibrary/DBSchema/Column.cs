
namespace DBComparerLibrary.DBSchema
{
    public class Column
    {
        public Column(string name, int type, int precision = 0, int scale = 0, int maxSymb = 0)
        {
            Name = name;
            Type = type;
            DopInfo = new ColumnInfo(precision,scale,maxSymb);
        }

        public string Name { get;}
        public int Type { get; }
        public ColumnInfo DopInfo;
    }
    public class ColumnInfo 
    {
        public SQLConstraintsEnum SQLConstraintsEnum;
        public int precision { get; }
        public int scale { get; }
        public int maxSymb { get; }

        public ColumnInfo(int precision = 0, int scale = 0, int maxSymb = 0, SQLConstraintsEnum constraintsEnum = SQLConstraintsEnum.none)
        {
            this.precision = precision;
            this.scale = scale;
            this.maxSymb = maxSymb;
            SQLConstraintsEnum = constraintsEnum;
        }
    }
}
