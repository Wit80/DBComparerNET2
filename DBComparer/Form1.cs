using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DBComparerLibrary.DBSQLExecutor;
using DBComparerLibrary.DBSchema;

namespace DBComparer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // string sSQL_objects = @"select name,object_id,schema_id,(select name from sys.schemas where sys.schemas.schema_id = sys.objects.schema_id) as Schema_Name,type,create_date,modify_date from sys.objects where type in ('U','V','PK','UQ','F')";
            string sSQL_objects = @"select o.name,
o.object_id,
o.schema_id,
s.name,
o.type,
o.create_date,
o.modify_date 
from sys.objects as o
INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
where o.type in ('U','V','PK','UQ','F')";
            //string sSQL_columns = @"select object_id,name,column_id, system_type_id, (select sys.types.name from sys.types where sys.types.user_type_id = sys.columns.user_type_id) as aa from sys.columns";
            string sSQL_columns = @"select c.object_id,
c.name,
c.column_id, 
c.system_type_id,
t.name  
from sys.columns as c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id";


            string sSQL_indexes = @"SELECT  o.object_id,
		o.schema_id,
		s.name as Schema_Name,
		o.type,
		o.create_date,
		o.modify_date,
        o.Name AS TableName ,
        i.Name AS IndexName
FROM    sys.objects o
        INNER JOIN sys.indexes i ON o.object_id = i.object_id
        INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
WHERE o.Type = 'U'-- User table
        AND LEFT(i.Name, 1) <> '_'
ORDER BY o.NAME ,
        i.name";


            string sSQL_records = @"SELECT  @@ServerName AS Server ,
        DB_NAME() AS DBName ,
        OBJECT_SCHEMA_NAME(p.object_id) AS SchemaName ,
        OBJECT_NAME(p.object_id) AS TableName ,
        i.Type_Desc ,
        i.Name AS IndexUsedForCounts ,
        SUM(p.Rows) AS Rows
FROM    sys.partitions p
        JOIN sys.indexes i ON i.object_id = p.object_id
                              AND i.index_id = p.index_id
WHERE   i.type_desc IN ( 'CLUSTERED', 'HEAP' )
        AND OBJECT_SCHEMA_NAME(p.object_id) <> 'sys'
GROUP BY p.object_id ,
        i.type_desc ,
        i.Name
ORDER BY SchemaName ,
        TableName";
            Dictionary<string, Schema> dictDB = new Dictionary<string, Schema>();

            string connSring = "Server = WIN-B080H6IP1JD;integrated security=true; database = AdventureWorks2019";
            ISQLGetConnection connection = new SQLDBConnection(connSring);
            string sSQL = "select name, schema_id from sys.schemas except select name, schema_id from sys.schemas where name like 'db_%' or name like 'dbo%' or name like 'sys%'  ";
            ISQLExecutor executor = new SQLExecutor();
            var res = executor.ExecuteSQL(connection.GetConnection(), sSQL);
            if (0 == res.Tables[0].Rows.Count) 
            {//нет схем
            }
            foreach (DataRow dr in res.Tables[0].Rows) 
            {
                dictDB.Add(dr[0].ToString(), new Schema(dr[0].ToString(), Convert.ToInt32(dr[1])));
            }
            foreach (Schema sh in dictDB.Values)
            {

            }







        }
    }
}
