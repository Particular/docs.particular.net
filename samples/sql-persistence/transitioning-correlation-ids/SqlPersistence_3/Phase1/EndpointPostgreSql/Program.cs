﻿using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;
using NServiceBus.Persistence.Sql;

partial class Program
{
    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.Title = "EndpointPostgreSql";

        var endpointConfiguration = new EndpointConfiguration("EndpointPostgreSql");

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        var password = Environment.GetEnvironmentVariable("PostgreSqlPassword");
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("Could not extract 'PostgreSqlPassword' from Environment variables.");
        }
        var username = Environment.GetEnvironmentVariable("PostgreSqlUserName");
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new Exception("Could not extract 'PostgreSqlUserName' from Environment variables.");
        }

        var connection = $"Host=localhost;Username={username};Password={password};Database=NsbSamplesSqlPersistenceTransition";

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
        dialect.JsonBParameterModifier(
            modifier: parameter =>
            {
                var npgsqlParameter = (NpgsqlParameter)parameter;
                npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
            });
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new NpgsqlConnection(connection);
            });
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        await SendMessage(endpointInstance)
            .ConfigureAwait(false);
        Console.WriteLine("StartOrder Message sent");

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}