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

        public DataBase(string dbServer, string dbName)
        {
            this.dbServer = dbServer;
            this.dbName = dbName;
            this.schemas = new Dictionary<string,Schema>();
        }

        public bool Equals(DataBase other)
        {
            if (other == null)
                return false;


            return CollectionComparer.DictEquals(this.schemas, other.schemas);


        }
    }
}
