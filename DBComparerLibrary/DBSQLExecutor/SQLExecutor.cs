using System;
using System.Collections.Generic;
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
        public static List<string> GetDbsFromServer(string connString)
        {

            SqlConnection _sqlConnection = new SqlConnection(connString);

            _sqlConnection.Open();
            SqlCommand command = new SqlCommand("select name from sys.databases", _sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<string> schReturn = new List<string>();
            while (reader.Read())
            {
                schReturn.Add(reader[0].ToString());
            }
            return schReturn;
        }
    }
}
