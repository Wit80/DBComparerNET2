using System.Data;
using System.Data.SqlClient;

namespace DBComparerLibrary.DBSQLExecutor
{
    public interface ISQLExecutor
    {
        DataSet ExecuteSQL(SqlConnection conn, string sSQL);
        DataSet ExecuteSQL(SqlConnection conn, string sSQL1, string sSQL2, string sSQL3, string sSQL4);
    }
}
