using System.Data;
using System.Data.SqlClient;

namespace DBComparerLibrary.DBSQLExecutor
{
    public interface ISQLExecutor
    {
        DataSet ExecuteSQL(SqlConnection conn, string sSQL);
    }
}
