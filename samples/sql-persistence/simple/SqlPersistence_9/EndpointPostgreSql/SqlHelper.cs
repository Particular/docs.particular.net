using System.Data.Common;
using Npgsql;

namespace EndpointPostgreSql;

public static class SqlHelper
{
    public static void EnsureDatabaseExists(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var database = builder.Database;

        var postgresDbConnection = connectionString.Replace(builder.Database, "postgres");

        using var connection = new NpgsqlConnection(postgresDbConnection);
        connection.Open();

        if (DatabaseIsMissing(connection, database))
        {
            using var command = connection.CreateCommand();

            command.CommandText = $"CREATE DATABASE \"{database}\"";
            command.ExecuteNonQuery();
        }
    }

    static bool DatabaseIsMissing(DbConnection connection, string database)
    {
        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{database}'";

        return command.ExecuteScalar() == null;
    }
}
