using System.Data.SqlClient;

public static class SqlHelper
{
    public static string ConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlNative;Integrated Security=True;Max Pool Size=100";

    static SqlHelper()
    {
        EnsureDatabaseExists();
    }

    static void EnsureDatabaseExists()
    {
        var builder = new SqlConnectionStringBuilder(ConnectionString);
        var database = builder.InitialCatalog;

        var masterConnection = ConnectionString.Replace(builder.InitialCatalog, "master");

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