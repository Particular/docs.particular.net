namespace Operations.SqlServer
{
    using System;
    using System.Data.SqlClient;

    public static class QueueCreation
    {
        public static void CreateQueuesForEndpoint()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";

            #region sqlserver-create-queues-endpoint-usage

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                CreateQueuesForEndpoint(
                    connection: sqlConnection,
                    schema: "dbo",
                    endpointName: "myendpoint");
            }

            #endregion

            #region sqlserver-create-queues-shared-usage

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                CreateQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "error");

                CreateQueue(
                    connection: sqlConnection,
                    schema: "dbo",
                    queueName: "audit");
            }

            #endregion
        }

        #region sqlserver-create-queues

        public static void CreateQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
        {
            //main queue
            CreateQueue(connection, schema, endpointName);

            //callback queue
            CreateQueue(connection, schema, endpointName + "." + Environment.MachineName);

            //retries queue
            CreateQueue(connection, schema, endpointName + ".Retries");

            //timeout queue
            CreateQueue(connection, schema, endpointName + ".Timeouts");

            //timeout dispatcher queue
            CreateQueue(connection, schema, endpointName + ".TimeoutsDispatcher");
        }

        public static void CreateQueue(SqlConnection connection, string schema, string queueName)
        {
            string createQueueScript =
                @"IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'U'))
                  BEGIN
                    CREATE TABLE [{0}].[{1}](
	                    [Id] [uniqueidentifier] NOT NULL,
	                    [CorrelationId] [varchar](255),
	                    [ReplyToAddress] [varchar](255),
	                    [Recoverable] [bit] NOT NULL,
	                    [Expires] [datetime],
	                    [Headers] [varchar](max) NOT NULL,
	                    [Body] [varbinary](max),
	                    [RowVersion] [bigint] IDENTITY(1,1) NOT NULL
                    ) ON [PRIMARY];                    
                    CREATE CLUSTERED INDEX [Index_RowVersion] ON [{0}].[{1}] 
                    (
	                    [RowVersion] ASC
                    ) ON [PRIMARY]
                  END";

            string sql = string.Format(createQueueScript, schema, queueName);
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

    #endregion
    }

}