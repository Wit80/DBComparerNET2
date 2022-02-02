using System;
using System.Data;
using System.Data.SqlClient;

namespace DBComparerLibrary.DBSQLExecutor
{
    public class SQLExecutor : ISQLExecutor
    {
        public  DataSet ExecuteSQL(SqlConnection conn, string sSQL)
        {
            DataSet dsRet  = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(sSQL, conn);
            try
            {
                adapter.Fill(dsRet);
                conn.Close();
                return dsRet;
            }
            catch (InvalidOperationException ex) 
            {
                throw new ComparerException("Ошибка InvalidOperationException при заполнении DataSet: Тип исключения: " + ex.GetType() + " : " + ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new ComparerException("Ошибка ArgumentException при заполнении DataSet: Тип исключения: " + ex.GetType() + " : " + ex.Message, ex);
            }
        }
    }
}
