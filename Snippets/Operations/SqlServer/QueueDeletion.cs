namespace Operations.SqlServer
{
    using System;
    using System.Data.SqlClient;

    #region sqlserver-delete-queues

    public static class QueueDeletion
    {

        public static void DeleteQueue(SqlConnection connection, string schema, string queueName)
        {
            string sql = @"
                    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'U'))
                    DROP TABLE [{0}].[{1}]";
            string deleteScript = string.Format(sql, schema, queueName);
            using (var command = new SqlCommand(deleteScript, connection))
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

    }

    #endregion
}