using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class DataBase
    {
        public string dbServer { get; }
        public string dbName { get; }
        public Dictionary<string, Schema> schemas { get; }

        public DataBase(string dbServer, string dbName)
        {
            this.dbServer = dbServer;
            this.dbName = dbName;
            this.schemas = new Dictionary<string,Schema>();
        }
    }
}
