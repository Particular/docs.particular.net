using System.Data.SqlClient;
using System.Threading.Tasks;

public static class SqlHelper
{
    public static async Task ExecuteSql(string connection, string sql)
    {
        using (var sqlConnection = new SqlConnection(connection))
        {
            await sqlConnection.OpenAsync()
                .ConfigureAwait(false);

            using (var command = sqlConnection.CreateCommand())
            {
                command.CommandText = sql;
                await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}