using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    internal class Column
    {
        public Column(string name, SQLDateTypeEnum type, ColumnInfo dopInfo = null)
        {
            Name = name;
            Type = type;
            DopInfo = dopInfo;
        }

        public string Name { get;}
        public SQLDateTypeEnum Type { get; }
        public ColumnInfo DopInfo { get; }
    }
    internal class ColumnInfo 
    {
        public SQLConstraintsEnum SQLConstraintsEnum { get; }
        public int param1 { get; }
        public int param2 { get; }
        public string UserTypeName { get; }

        public ColumnInfo(int param1 = 0, int param2 = 0, string userTypeName = "", SQLConstraintsEnum constraintsEnum = SQLConstraintsEnum.none)
        {
            this.param1 = param1;
            this.param2 = param2;
            UserTypeName = userTypeName;
            SQLConstraintsEnum = constraintsEnum;
        }
    }
}
