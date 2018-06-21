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

                    //Exectute SQL statement
                    sqlCommand.ExecuteNonQuery();

                    var options = new SendOptions();

                    options.UseCustomSqlConnectionAndTransaction(connection, transaction);

                    //Send bunch of messages using the same transaction
                    await session.Send(new Message(), options);
                    await session.Send(new Message(), options);
                    await session.Send(new Message(), options);

                    transaction.Commit();
                }
            }

            #endregion

        } 
        
        class Message{};
    }
}