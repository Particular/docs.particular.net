using System.Data.SqlClient;
using System.Threading.Tasks;

public static class SqlHelper
{
    public static async Task ExecuteSql(string connectionString, string sql)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync().ConfigureAwait(false);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }
}