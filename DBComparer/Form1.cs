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
