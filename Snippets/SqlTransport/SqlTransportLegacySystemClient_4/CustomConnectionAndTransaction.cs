namespace SqlTransport_4
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Transport.SQLServer;

    class CustomConnectionAndTransaction
    {
        async Task Usage(IMessageSession session, string connectionString, string commandText)
        {
            #region UseCustomSqlConnectionAndTransaction [4.1,)

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
                    sendOptions.UseCustomSqlConnectionAndTransaction(connection, transaction);
                    await session.Send(new Message(), sendOptions);

                    transaction.Commit();
                }
            }

            #endregion

        } 
        
        class Message{};
    }
}