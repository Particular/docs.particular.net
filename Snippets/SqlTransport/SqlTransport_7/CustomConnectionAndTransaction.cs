using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using NServiceBus;

class CustomConnectionAndTransaction
{
    async Task Usage(IMessageSession session, string connectionString, string commandText)
    {
        #region UseCustomSqlConnectionAndTransaction

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var sqlCommand = new SqlCommand(commandText, connection, transaction);

                //Execute SQL statement
                sqlCommand.ExecuteNonQuery();

                //Send a message
                var sendOptions = new SendOptions();
                sendOptions.UseCustomSqlTransaction(transaction);
                await session.Send(new Message(), sendOptions);

                //Publish a message
                var publishOptions = new PublishOptions();
                publishOptions.UseCustomSqlTransaction(transaction);
                await session.Publish(new Event(), publishOptions);

                transaction.Commit();
            }
        }

        #endregion

        #region UseCustomSqlConnection

        using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, 
            TransactionScopeAsyncFlowOption.Enabled))
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sqlCommand = new SqlCommand(commandText, connection);

                //Execute SQL statement
                sqlCommand.ExecuteNonQuery();

                //Send a message
                var sendOptions = new SendOptions();
                sendOptions.UseCustomSqlConnection(connection);
                await session.Send(new Message(), sendOptions);

                //Publish a message
                var publishOptions = new PublishOptions();
                publishOptions.UseCustomSqlConnection(connection);
                await session.Publish(new Event(), publishOptions);
            }

            scope.Complete();
        }

        #endregion
    }

    class Message { };
    class Event { };
}