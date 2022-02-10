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

        public DataSet ExecuteSQL(SqlConnection conn, string sSQL1, string sSQL2, string sSQL3, string sSQL4) 
        {
            DataSet dsRet = new DataSet();
            try
            {
                SqlDataAdapter adapter1 = new SqlDataAdapter(sSQL1, conn);
                SqlDataAdapter adapter2 = new SqlDataAdapter(sSQL2, conn);
                SqlDataAdapter adapter3 = new SqlDataAdapter(sSQL3, conn); 
                SqlDataAdapter adapter4 = new SqlDataAdapter(sSQL4, conn);
                adapter1.Fill(dsRet,"Table1");
                adapter2.Fill(dsRet,"Table2");
                adapter3.Fill(dsRet,"Table3");
                adapter4.Fill(dsRet,"Table4");
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
            SqlCommand command;
            SqlDataReader reader;

            try
            {
                _sqlConnection.Open();
            }
            catch (Exception ex)
            {
                return null;
            }

            command = new SqlCommand("select name from sys.databases", _sqlConnection);
            reader = command.ExecuteReader();
            
            List<string> schReturn = new List<string>();
            while (reader.Read())
            {
                schReturn.Add(reader[0].ToString());
            }
            return schReturn;
        }
    }
}
