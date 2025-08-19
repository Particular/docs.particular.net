using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NServiceBus;

public static class Helper
{
    public static void EnsureDatabaseExists(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var database = builder.InitialCatalog;

        var masterConnection = connectionString.Replace(builder.InitialCatalog, "master");

        using (var connection = new SqlConnection(masterConnection))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"""
                                       if(db_id('{database}') is null)
                                           create database [{database}]
                                       """;
                command.ExecuteNonQuery();
            }
        }

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = """
                                      if not exists (select * from sys.tables where name = 'BusinessObject')
                                      create table BusinessObject ( Id int identity(1,1) not null primary key, MessageId varchar(40) not null)
                                      """;

                command.ExecuteNonQuery();
            }
        }
    }

    public static Task SendDuplicates<TMessage>(IMessageSession context, TMessage message, int totalCount)
    {
        var duplicatedMessageId = Guid.NewGuid().ToString();

        var tasks = Enumerable.Range(0, totalCount)
            .Select(i =>
            {
                var options = new SendOptions();
                options.RouteToThisEndpoint();
                options.SetMessageId(duplicatedMessageId);

                return context.Send(message, options);
            });

        return Task.WhenAll(tasks);
    }
}