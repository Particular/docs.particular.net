namespace SqlServer_All.Operations
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Transactions;

    public static class ErrorQueue
    {

        static void Usage()
        {
            #region sqlserver-return-to-source-queue-usage

            ReturnMessageToSourceQueue(
                errorQueueConnectionString: @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True",
                errorQueueName: "errors",
                retryConnectionString: @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True",
                retryQueueName: "target",
                messageId: Guid.Parse("1667B60E-2948-4EF0-8BB1-8C851A9407D2")
                );

            #endregion
        }

        #region sqlserver-return-to-source-queue

        public static void ReturnMessageToSourceQueue(
            string errorQueueConnectionString,
            string errorQueueName,
            string retryConnectionString,
            string retryQueueName,
            Guid messageId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                MessageToRetry messageToRetry = ReadAndDelete(errorQueueConnectionString, errorQueueName, messageId);
                RetryMessage(retryConnectionString, retryQueueName,  messageToRetry);
                scope.Complete();
            }
        }

        class MessageToRetry
        {
            public Guid Id;
            public string Headers;
            public byte[] Body;
        }

        static void RetryMessage(string connectionString, string queueName, MessageToRetry messageToRetry)
        {
            string sql = string.Format(
                @"INSERT INTO [{0}] (
                        [Id],
                        [Recoverable],
                        [Headers],
                        [Body])
                    VALUES (
                        @Id,
                        @Recoverable,
                        @Headers,
                        @Body)", queueName);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlParameterCollection parameters = command.Parameters;
                    command.CommandType = CommandType.Text;
                    parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = messageToRetry.Id;
                    parameters.Add("Headers", SqlDbType.VarChar).Value = messageToRetry.Headers;
                    parameters.Add("Body", SqlDbType.VarBinary).Value = messageToRetry.Body;
                    parameters.Add("Recoverable", SqlDbType.Bit).Value = true;
                    command.ExecuteNonQuery();
                }
            }
        }

        static MessageToRetry ReadAndDelete(string connectionString, string queueName, Guid messageId)
        {
            string sql = string.Format(
                @"DELETE FROM [{0}]
                OUTPUT
                   DELETED.Headers,
                   DELETED.Body
                WHERE Id = @Id", queueName);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Id", messageId);
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!reader.Read())
                        {
                            string message = string.Format("Could not find error entry with messageId '{0}'", messageId);
                            throw new Exception(message);
                        }
                        return new MessageToRetry
                        {
                            Id = messageId,
                            Headers = reader.GetString(0),
                            Body = reader.GetSqlBinary(1).Value
                        };
                    }
                }
            }
        }

        #endregion
    }
}
