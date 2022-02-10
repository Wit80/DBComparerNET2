using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class ddDataBase : IEquatable<ddDataBase>
    {
        public string dbServer { get; }// не участвует в сравнении
        public string dbName { get; }// не участвует в сравнении
        public Dictionary<string, ddSchema> schemas { get; }

        public ddDataBase(string dbServer, string dbName)
        {
            this.dbServer = dbServer;
            this.dbName = dbName;
            this.schemas = new Dictionary<string,ddSchema>();
        }

        public bool Equals(ddDataBase other)
        {
            if (other == null)
                return false;


            return Comparer.DictEquals(this.schemas, other.schemas);


        }
    }
}
