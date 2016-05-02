using System;
using System.Data.SqlClient;

public static class QueueDeletion
{
    public static void DeleteQueuesForEndpoint()
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";

        #region sqlserver-delete-queues-endpoint-usage

        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.Open();
            DeleteQueuesForEndpoint(
                connection: sqlConnection,
                schema: "dbo",
                endpointName: "myendpoint");
        }

        #endregion
    }

    public static void DeleteSharedQueues()
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";

        #region sqlserver-delete-queues-shared-usage

        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.Open();
            DeleteQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "audit");
            DeleteQueue(
                connection: sqlConnection,
                schema: "dbo",
                queueName: "error");
        }

        #endregion
    }

    #region sqlserver-delete-queues

    public static void DeleteQueue(SqlConnection connection, string schema, string queueName)
    {
        string sql = @"
                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'U'))
                DROP TABLE [{0}].[{1}]";
        string deleteScript = string.Format(sql, schema, queueName);
        using (SqlCommand command = new SqlCommand(deleteScript, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    public static void DeleteQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
    {
        //main queue
        DeleteQueue(connection, schema, endpointName);

        //callback queue
        DeleteQueue(connection, schema, endpointName + "." + Environment.MachineName);

        //retries queue
        DeleteQueue(connection, schema, endpointName + ".Retries");

        //timeout queue
        DeleteQueue(connection, schema, endpointName + ".Timeouts");

        //timeout dispatcher queue
        DeleteQueue(connection, schema, endpointName + ".TimeoutsDispatcher");
    }

    #endregion
}