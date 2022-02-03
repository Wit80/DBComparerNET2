using DBComparerLibrary.DBSchema;
using DBComparerLibrary.DBSQLExecutor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace DBComparerLibrary
{
    public class DBsysWorker
    {
        public DataSet dsObjects;
        public int ObjectsOKflag = 0;
        public string ObjectsExceptionText;
        //public DataSet dsColumns;
        public Dictionary<UInt64, Dictionary<string, Column>> dictColumns;
        public int ColumnsOKflag = 0;
        public string ColumnsExceptionText;
        //public DataSet dsIndexes;
        public Dictionary<UInt64, Dictionary<string, Index>> dictIndexes;
        public int IndexesOKflag = 0;
        public string IndexesExceptionText;
        //public DataSet dsRowsCount;
        public Dictionary<UInt64, int> dictRowsCount;
        public int RowsCountOKflag = 0;
        public string RowsCountExceptionText;
        public Dictionary<int, string> dictTypes;
        public int TypesOKflag = 0;
        public string TypesExceptionText;

        private string _connString;


        

        public DBsysWorker(string connString)
        {
            _connString = connString;
        }
        public int GetRowCount(UInt64 objId) 
        {
            if (dictRowsCount.ContainsKey(objId))
                return dictRowsCount[objId];
            else 
                return 0;
        }
        public Dictionary<string, Column> GetColums(UInt64 objId) 
        {
            if (dictColumns.ContainsKey(objId))
                return dictColumns[objId];
            else
                return null;
        }
        public Dictionary<string, Index> GetIndexes(UInt64 objId) 
        {
            if (dictIndexes.ContainsKey(objId))
                return dictIndexes[objId];
            else
                return null;
        }
        public void GetDataFromDB() 
        {
            Thread t_sys = new Thread(new ThreadStart(GetSysObjects));
            Thread t_col = new Thread(new ThreadStart(GetSysColumns));
            Thread t_ind = new Thread(new ThreadStart(GetSysIndexes));
            Thread t_rows = new Thread(new ThreadStart(GetSysRowsCount));
            Thread t_types = new Thread(new ThreadStart(GetSysTypes));

            t_sys.Start();
            t_col.Start();
            t_ind.Start();
            t_rows.Start();
            t_types.Start();
            Thread.Sleep(100);

            while (!(1 == ColumnsOKflag && 1 == ObjectsOKflag && 1 == IndexesOKflag && 1 == RowsCountOKflag && 1 == TypesOKflag))
            {
                if (-1 == ObjectsOKflag)
                    throw new ComparerException(ObjectsExceptionText);
                if(-1 == ColumnsOKflag)
                    throw new ComparerException(ColumnsExceptionText);
                if (-1 == IndexesOKflag)
                    throw new ComparerException(IndexesExceptionText);
                if (-1 == RowsCountOKflag)
                    throw new ComparerException(RowsCountExceptionText);
                if (-1 == TypesOKflag)
                    throw new ComparerException(TypesExceptionText);

                Thread.Sleep(1000);

            }
        }
        private void GetSysObjects() 
        {
            string sSQL = @"
SELECT @@ServerName AS serverName,
        DB_NAME() AS dbName,
        o.name as objectName,
        o.object_id as objectId,
        o.schema_id as schemaId,
        s.name as schemaName,
        o.type as objectType,
        o.create_date as dtCreate,
        o.modify_date as dtModif,
		o.parent_object_id as parentObjId,
		' ' as tblName,
		GETDATE() as tblCreate,
		GETDATE() as tblModify
FROM sys.objects as o
        INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
WHERE o.type in ('U','V','PK','UQ','F','C') and o.parent_object_id = 0
union all
SELECT @@ServerName AS serverName,
        DB_NAME() AS dbName,
        o.name as objectName,
        o.object_id as objectId,
        o.schema_id as schemaId,
        s.name as schemaName,
        o.type as objectType,
        o.create_date as dtCreate,
        o.modify_date as dtModif,
		o.parent_object_id as parentObjId,
		t.name as tblName,
		t.create_date as tblCreate,
		t.modify_date as tblModify
FROM sys.objects as o
        INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
		INNER JOIN sys.tables t ON o.parent_object_id = t.object_id
WHERE o.type in ('U','V','PK','UQ','F','C') ";
            try
            {
                dsObjects = execute(sSQL);
            }
            catch (Exception ex) 
            {
                ObjectsOKflag = -1;
                ObjectsExceptionText = "Ошибка GetSysObjects. Тип исключения: " + ex.GetType() + " : " + ex.Message;
            }
            ObjectsOKflag = 1;
        }

        private void GetSysColumns()
        {
            string sSQL = @"
SELECT  c.object_id as objectId,
        c.name as columnName,
        c.system_type_id as columnTypeId,
        t.name as columnTypName,
		c.precision,
		c.scale,
		columnproperty(c.object_id, c.name, 'Precision') maxSymbols
FROM sys.columns as c
        INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
        INNER JOIN sys.objects o ON c.object_id = o.object_id
    INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
	order by s.name, o.name";
            try
            {
                dictColumns = GetColumnsDictionary(execute(sSQL));
            }
            catch (Exception ex)
            {
                ColumnsOKflag = -1;
                ColumnsExceptionText = "Ошибка GetSysColumns. Тип исключения: " + ex.GetType() + " : " + ex.Message;
            }
            ColumnsOKflag = 1;
        }
        private void GetSysIndexes()
        {
            string sSQL = @"
SELECT  o.object_id as objectId,
        o.create_date as dtCreate,
        o.modify_date as dtModif,
        i.Name AS indexName,
		COL_NAME(ic.object_id,ic.column_id) AS columnName
FROM    sys.objects o
        INNER JOIN sys.indexes i ON o.object_id = i.object_id
        INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
		INNER JOIN sys.index_columns AS ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id  
WHERE o.Type = 'U'
        AND LEFT(i.Name, 1) <> '_'";
            try
            {
                dictIndexes = GetIndexesDictionary(execute(sSQL));
            }
            catch (Exception ex)
            {
                IndexesOKflag = -1;
                IndexesExceptionText = "Ошибка GetSysIndexes. Тип исключения: " + ex.GetType() + " : " + ex.Message;
            }
            IndexesOKflag = 1;
        }

        private void GetSysRowsCount()
        {
            string sSQL = @"
select p.object_id as objectId,
        SUM(p.Rows) AS rowsCount
FROM sys.partitions p
        JOIN sys.indexes i ON i.object_id = p.object_id
                              AND i.index_id = p.index_id
WHERE i.type_desc IN ( 'CLUSTERED', 'HEAP' )
        AND OBJECT_SCHEMA_NAME(p.object_id) <> 'sys'
GROUP BY p.object_id ,
        i.type_desc ,
        i.Name";
            try
            {
                dictRowsCount = GetRowsCountDictionary(execute(sSQL));
            }
            catch (Exception ex)
            {
                RowsCountOKflag = -1;
                RowsCountExceptionText = "Ошибка GetSysRowsCount. Тип исключения: " + ex.GetType() + " : " + ex.Message;
            }
            RowsCountOKflag = 1;
        }
        private void GetSysTypes()
        {
            string sSQL = @"
select t.user_type_id as typeID,
		s.name + '.' + t.name as typeName
from sys.types as t
	INNER JOIN sys.schemas s ON t.schema_id = s.schema_id 
	where s.name != 'sys'
	union all
select t.user_type_id as typeID,
		t.name  as typeName
from sys.types as t
	INNER JOIN sys.schemas s ON t.schema_id = s.schema_id 
	where s.name = 'sys'";
            try
            {
                dictTypes = GetTypesDictionary(execute(sSQL));
            }
            catch (Exception ex)
            {
                TypesOKflag = -1;
                TypesExceptionText = "Ошибка GetSysTypes. Тип исключения: " + ex.GetType() + " : " + ex.Message;
            }
            TypesOKflag = 1;
        }

        private DataSet execute(string sSQL) 
        {
            ISQLGetConnection _connection = new SQLDBConnection(_connString);
            ISQLExecutor _executor = new SQLExecutor();
            return _executor.ExecuteSQL(_connection.GetConnection(), sSQL);
        }
        /* ОБРАБОТКА КОЛИЧЕСТВА ЗАПИСЕЙ*/
        private Dictionary<UInt64, int> GetRowsCountDictionary(DataSet ds)
        {
            Dictionary<UInt64, int> dictRet = new Dictionary<UInt64, int>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dictRet.Add(Convert.ToUInt64(dr[0]), Convert.ToInt32(dr[1]));
            }
            return dictRet;
        }
        /*ОБРАБОТКА ИНДЕКСОВ*/
        private Dictionary<UInt64, Dictionary<string, Index>> GetIndexesDictionary(DataSet ds)
        {
            Dictionary<UInt64, Dictionary<string, Index>> dictRet = new Dictionary<UInt64, Dictionary<string, Index>>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dictRet.ContainsKey(Convert.ToUInt64(dr[0])))
                {//эта таблица уже обрабатывалась
                    if (dictRet[Convert.ToUInt64(dr[0])].ContainsKey(dr[3].ToString()))
                    {//этот индекс встречался, добавляес столбец
                        dictRet[Convert.ToUInt64(dr[0])][dr[3].ToString()].columns.Add(dr[4].ToString());
                    }
                    else
                    {
                        dictRet[Convert.ToUInt64(dr[0])].Add(dr[3].ToString(), new Index(dr[3].ToString(), Convert.ToDateTime(dr[1]), Convert.ToDateTime(dr[2]), dr[4].ToString()));
                    }
                }
                else
                {
                    dictRet.Add(Convert.ToUInt64(dr[0]), new Dictionary<string, Index>() { { dr[3].ToString(), new Index(dr[3].ToString(), Convert.ToDateTime(dr[1]), Convert.ToDateTime(dr[2]), dr[4].ToString()) } });
                }
            }
            return dictRet;
        }
        /*ОБРАБОТКА СТОЛБЦОВ*/
        private Dictionary<UInt64, Dictionary<string, Column>> GetColumnsDictionary(DataSet ds)
        {
            Dictionary<UInt64, Dictionary<string, Column>> dictRet = new Dictionary<UInt64, Dictionary<string, Column>>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dictRet.ContainsKey(Convert.ToUInt64(dr[0])))
                {// эту таблицу уже встречали
                    if (!dictRet[Convert.ToUInt64(dr[0])].ContainsKey(dr[1].ToString()))
                    {
                        dictRet[Convert.ToUInt64(dr[0])].Add(dr[1].ToString(),
                            new Column(dr[1].ToString(), Convert.ToInt32(dr[2]), Convert.ToInt32(dr[4]), Convert.ToInt32(dr[5]), Convert.ToInt32(dr[6])));
                    }
                }
                else
                {
                    dictRet.Add(Convert.ToUInt64(dr[0]),
                        new Dictionary<string, Column>() { { dr[1].ToString(),
                                new Column(dr[1].ToString(),Convert.ToInt32(dr[2]), Convert.ToInt32(dr[4]), Convert.ToInt32(dr[5]), Convert.ToInt32(dr[6])) } });
                }
            }
            return dictRet;
        }

        /*ОБРАБОТКА ТИПОВ*/
        private Dictionary<int, string> GetTypesDictionary(DataSet ds)
        {
            Dictionary<int, string> dictRet = new Dictionary<int, string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (!dictRet.ContainsKey(Convert.ToInt32(dr[0])))
                {
                    dictRet.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                }
            }
            return dictRet;
        }
    }
}
