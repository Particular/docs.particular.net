using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
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

                transaction.Commit();
            }
        }

        #endregion
    }

    class Message { };
}