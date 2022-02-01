using System;
using System.Data.SqlClient;

namespace DBComparerLibrary.DBSQLExecutor
{
    public class SQLDBConnection : ISQLGetConnection
    {
        private readonly SqlConnection _sqlConnection;
        private readonly string _sConnectionstring;

        public SQLDBConnection(string sConnectionstring)
        {
            if (null == sConnectionstring)
            {
                throw new ArgumentNullException("SQLConnection");
            }
            _sConnectionstring = sConnectionstring;
            try
            {
                _sqlConnection = new SqlConnection(_sConnectionstring);
            }
            catch (InvalidOperationException ex)
            {
                throw new ComparerException("Ошибка InvalidOperationException при установлении соединения с MSSQL: Тип исключения:" + ex.GetType() + " : " + ex.Message, ex);
            }

        }

        public SqlConnection GetConnection()
        {
            try
            {
                if (System.Data.ConnectionState.Open != _sqlConnection.State)
                {
                    _sqlConnection.Open();
                }
            }
            catch (InvalidOperationException ex) 
            {
                throw new ComparerException("Ошибка InvalidOperationException при открытии соединения с MSSQL: Тип исключения:" + ex.GetType() + " : " + ex.Message,ex);
            }
            return _sqlConnection;
        }
    }
}
