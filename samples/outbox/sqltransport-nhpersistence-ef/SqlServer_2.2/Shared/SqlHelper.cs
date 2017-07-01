using System.Data.SqlClient;

public static class SqlHelper
{
    public static void ExecuteSql(string connectionString, string sql)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }
    }
}