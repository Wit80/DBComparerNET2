using System.Data.SqlClient;

namespace DBComparerLibrary.DBSQLExecutor
{
    public interface ISQLGetConnection
    {
        SqlConnection GetConnection();
    }
}
