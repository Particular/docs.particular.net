using MySql.Data.MySqlClient;

public static class SqlHelper
{
    public static void EnsureDatabaseExists(string connectionString)
    {
        var builder = new MySqlConnectionStringBuilder(connectionString);
        var database = builder.Database;

        var mysqlDbConnection = connectionString.Replace(builder.Database, "mysql");

        using var connection = new MySqlConnection(mysqlDbConnection);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $@"
    create database if not exists {database}
";
        command.ExecuteNonQuery();
    }
}
