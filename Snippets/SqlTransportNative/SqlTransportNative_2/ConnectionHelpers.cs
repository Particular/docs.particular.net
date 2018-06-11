using System.Data.SqlClient;
using System.Threading.Tasks;

public static class ConnectionHelpersImp
{
    #region ConnectionHelpers

    public static async Task<SqlConnection> OpenConnection(string connectionString)
    {
        var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync()
                .ConfigureAwait(false);
            return connection;
        }
        catch
        {
            connection.Dispose();
            throw;
        }
    }

    public static async Task<SqlTransaction> BeginTransaction(string connectionString)
    {
        var connection = await OpenConnection(connectionString)
            .ConfigureAwait(false);
        return connection.BeginTransaction();
    }

    #endregion
}