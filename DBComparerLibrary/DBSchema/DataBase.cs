using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class DataBase : IEquatable<DataBase>
    {
        public string dbServer { get; }// не участвует в сравнении
        public string dbName { get; }// не участвует в сравнении
        public SortedDictionary<string, Schema> schemas { get; }
        public SortedDictionary<string, Table> tables { get; }
        public SortedDictionary<string, View> views { get; }
        public DataBase(string dbServer, string dbName, SortedDictionary<string, Schema> schs, SortedDictionary<string, Table> tbls, SortedDictionary<string, View> vws)
        {
            this.dbServer = dbServer.Trim();
            this.dbName = dbName.Trim();
            schemas = schs;
            tables = tbls;
            views = vws;
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
