using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class DataBase : IEquatable<DataBase>
    {
        public string dbServer { get; }// не участвует в сравнении
        public string dbName { get; }// не участвует в сравнении
        public Dictionary<string, Schema> schemas { get; }
        public Dictionary<string, Table> tables { get; }
        public Dictionary<string, View> views { get; }
        public DataBase(string dbServer, string dbName)
        {
            this.dbServer = dbServer.Trim();
            this.dbName = dbName.Trim();
            schemas = new Dictionary<string, Schema>();
            tables = new Dictionary<string, Table>();
            views = new Dictionary<string, View>();
        }

        public bool Equals(DataBase other)
        {
            if (other == null)
                return false;


            return Comparer.DictEquals(this.schemas, other.schemas)
                &&
                Comparer.DictEquals(this.tables, other.tables)
                &&
                Comparer.DictEquals(this.views, other.views);
        }
    }
}
