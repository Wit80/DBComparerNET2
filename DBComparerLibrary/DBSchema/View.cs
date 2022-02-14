using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class View : IEquatable<View>
    {
        public View(string viewName, string script, Dictionary<string, Column> columns, List<string> tbl)
        {
            ViewName = viewName.Trim();
            Script = script.Trim();
            this.columns = columns;
            tables = tbl;
        }

        public string ViewName { get; }
        public string Script { get; } // не участвует в сравнении
        public Dictionary<string,Column> columns { get; }
        public List<string> tables { get; }
        public bool Equals(View other)
        {
            if (other == null)
                return false;

            return Comparer.DictEquals(this.columns, other.columns) &&
                Comparer.EnumEquals(this.tables, other.tables) &&
                Comparer.CompareStrings(this.ViewName, other.ViewName);
        }
    }
}
