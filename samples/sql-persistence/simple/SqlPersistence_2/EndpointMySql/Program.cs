﻿using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlPersistence.EndpointMySql";

        #region MySqlConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.EndpointMySql");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

        var password = Environment.GetEnvironmentVariable("MySqlPassword");
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("Could not extract 'MySqlPassword' from Environment variables.");
        }
        var username = Environment.GetEnvironmentVariable("MySqlUserName");
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new Exception("Could not extract 'MySqlUserName' from Environment variables.");
        }
        var connection = $"server=localhost;user={username};database=sqlpersistencesample;port=3306;password={password};AllowUserVariables=True;AutoEnlist=false";
        persistence.SqlVariant(SqlVariant.MySql);
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new MySqlConnection(connection);
            });
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        #endregion

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}