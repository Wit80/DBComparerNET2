using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
   
    public class Table : IEquatable<Table>
    {
        public Table(string tableName, Dictionary<string, Column> columns, Dictionary<string, Index> indexes, Dictionary<string, ForeignKey> fk,PrimaryKey pk)
        {
            TableName = tableName.Trim();
            this.columns = columns;
            this.indexes = indexes;
            this.foreignKeys = fk;
            PrimaryKey = pk;
        }

        public string TableName { get; }
        public PrimaryKey PrimaryKey { get; }
        public Dictionary<string, Column> columns { get; }
        public Dictionary<string, Index> indexes { get; }
        public Dictionary<string, ForeignKey> foreignKeys { get; }
        public bool Equals(Table other)
        {
            if (other == null)
                return false;

            return PrimaryKey.Equals(other.PrimaryKey) &&
                Comparer.DictEquals(this.columns, other.columns) &&
                Comparer.DictEquals(this.indexes, other.indexes) &&
                Comparer.DictEquals(this.foreignKeys, other.foreignKeys) &&
                Comparer.CompareStrings(this.TableName, other.TableName);
        }
    }
}
