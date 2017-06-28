using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;

namespace SqlServer_All.Operations.ErrorQueue
{
    public static class ErrorQueue
    {

        static async Task Usage()
        {
            #region sqlserver-return-to-source-queue-usage

            await ReturnMessageToSourceQueue(
                    errorQueueConnection: @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True",
                    errorQueueName: "errors",
                    retryConnectionString: @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True",
                    retryQueueName: "target",
                    messageId: Guid.Parse("1667B60E-2948-4EF0-8BB1-8C851A9407D2")
                )
                .ConfigureAwait(false);

            #endregion
        }

        #region sqlserver-return-to-source-queue

        public static async Task ReturnMessageToSourceQueue(
            string errorQueueConnection,
            string errorQueueName,
            string retryConnectionString,
            string retryQueueName,
            Guid messageId)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var messageToRetry = await ReadAndDelete(errorQueueConnection, errorQueueName, messageId)
                    .ConfigureAwait(false);
                await RetryMessage(retryConnectionString, retryQueueName,  messageToRetry)
                    .ConfigureAwait(false);
                scope.Complete();
            }
        }

        class MessageToRetry
        {
            public Guid Id;
            public string Headers;
            public byte[] Body;
        }

        static async Task RetryMessage(string connectionString, string queueName, MessageToRetry messageToRetry)
        {
            var sql = $@"
                insert into [{queueName}] (
                    Id,
                    Recoverable,
                    Headers,
                    Body)
                values (
                    @Id,
                    @Recoverable,
                    @Headers,
                    @Body)";
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync()
                    .ConfigureAwait(false);
                using (var command = new SqlCommand(sql, connection))
                {
                    var parameters = command.Parameters;
                    parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = messageToRetry.Id;
                    parameters.Add("Headers", SqlDbType.VarChar).Value = messageToRetry.Headers;
                    parameters.Add("Body", SqlDbType.VarBinary).Value = messageToRetry.Body;
                    parameters.Add("Recoverable", SqlDbType.Bit).Value = true;
                    await command.ExecuteNonQueryAsync()
                        .ConfigureAwait(false);
                }
            }
        }

        static async Task<MessageToRetry> ReadAndDelete(string connectionString, string queueName, Guid messageId)
        {
            var sql = $@"
            delete from [{queueName}]
            output
                deleted.Headers,
                deleted.Body
            where Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync()
                    .ConfigureAwait(false);
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Id", messageId);
                    using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow)
                        .ConfigureAwait(false))
                    {
                        if (await reader.ReadAsync()
                            .ConfigureAwait(false))
                        {
                            return new MessageToRetry
                            {
                                Id = messageId,
                                Headers = reader.GetString(0),
                                Body = reader.GetSqlBinary(1).Value
                            };
                        }
                        var message = $"Could not find error entry with messageId '{messageId}'";
                        throw new Exception(message);
                    }
                }
            }
        }

        #endregion
    }
}