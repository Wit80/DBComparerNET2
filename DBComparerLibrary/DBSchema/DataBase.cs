using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public class DataBase : IEquatable<DataBase>
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

        public bool Equals(DataBase other)
        {
            if (other == null)
                return false;


            return (
                    object.ReferenceEquals(this.dbName, other.dbName) ||
                    this.dbName != null &&
                    this.dbName.Equals(other.dbName)
                ) &&
                (
                    object.ReferenceEquals(this.dbServer, other.dbServer) ||
                    this.dbServer != null &&
                    this.dbServer.Equals(other.dbServer)
                ) &&
                Comparers.DictEquals(this.schemas, other.schemas);


        }
    }
}
