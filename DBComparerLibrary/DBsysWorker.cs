using DBComparerLibrary.DBSQLExecutor;
using System;
using System.Data;
using System.Threading;

namespace DBComparerLibrary
{
    public class DBsysWorker
    {
        public DataSet dsObjects;
        public int ObjectsOKflag = 0;
        public string ObjectsExceptionText;
        public DataSet dsColumns;
        public int ColumnsOKflag = 0;
        public string ColumnsExceptionText;
        public DataSet dsIndexes;
        public int IndexesOKflag = 0;
        public string IndexesExceptionText;
        public DataSet dsRowsCount;
        public int RowsCountOKflag = 0;
        public string RowsCountExceptionText;
        private string _connString;


        

        public DBsysWorker(string connString)
        {
            _connString = connString;
        }
        public void GetDataFromDB() 
        {
            Thread t_sys = new Thread(new ThreadStart(GetSysObjects));
            Thread t_col = new Thread(new ThreadStart(GetSysColumns));
            Thread t_ind = new Thread(new ThreadStart(GetSysIndexes));
            Thread t_rows = new Thread(new ThreadStart(GetSysRowsCount));

            t_sys.Start();
            t_col.Start();
            t_ind.Start();
            t_rows.Start();

            while (!(1 == ColumnsOKflag && 1 == ObjectsOKflag && 1 == IndexesOKflag && 1 == RowsCountOKflag))
            {
                if (-1 == ObjectsOKflag)
                    throw new ComparerException(ObjectsExceptionText);
                if(-1 == ColumnsOKflag)
                    throw new ComparerException(ColumnsExceptionText);
                if (-1 == IndexesOKflag)
                    throw new ComparerException(IndexesExceptionText);
                if (-1 == RowsCountOKflag)
                    throw new ComparerException(RowsCountExceptionText);

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
        o.modify_date as dtModif
FROM sys.objects as o
        INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
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
                dsColumns = execute(sSQL);
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
                dsIndexes = execute(sSQL);
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
                dsRowsCount = execute(sSQL);
            }
            catch (Exception ex)
            {
                RowsCountOKflag = -1;
                RowsCountExceptionText = "Ошибка GetSysRowsCount. Тип исключения: " + ex.GetType() + " : " + ex.Message;
            }
            RowsCountOKflag = 1;
        }

        private DataSet execute(string sSQL) 
        {
            ISQLGetConnection _connection = new SQLDBConnection(_connString);
            ISQLExecutor _executor = new SQLExecutor();
            return _executor.ExecuteSQL(_connection.GetConnection(), sSQL);
        }
    }
}
