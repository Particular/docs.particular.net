using System.Data.SqlClient;

public static class SqlHelper
{
    static SqlHelper()
    {
        EnsureDatabaseExists(ConnectionString);
    }

    public static string ConnectionString = @"Data Source=.\SQLExpress;Database=SqlHttpPassthroughSample; Integrated Security=True;Max Pool Size=100;MultipleActiveResultSets=True";

    static void EnsureDatabaseExists(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var database = builder.InitialCatalog;

        var masterConnection = connectionString.Replace(builder.InitialCatalog, "master");

        using (var connection = new SqlConnection(masterConnection))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $@"
if(db_id('{database}') is null)
    create database [{database}]
";
                command.ExecuteNonQuery();
            }
        }
    }
}