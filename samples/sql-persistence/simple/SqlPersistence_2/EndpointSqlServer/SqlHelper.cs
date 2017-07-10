using System.Data.SqlClient;
using System.Threading.Tasks;

public static class SqlHelper
{
    public static async Task EnsureDatabaseExists(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var database = builder.InitialCatalog;

        var masterConnection = connectionString.Replace(builder.InitialCatalog, "master");

        using (var connection = new SqlConnection(masterConnection))
        {
            await connection.OpenAsync()
                .ConfigureAwait(false);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $@"
if(db_id('{database}') is null)
	create database [{database}]
";
                await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}