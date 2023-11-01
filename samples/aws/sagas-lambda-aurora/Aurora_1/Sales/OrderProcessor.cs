using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using MySqlConnector;
using NServiceBus;

public class OrderProcessor
{
    #region EndpointSetup

    private static readonly AwsLambdaSQSEndpoint endpoint = new AwsLambdaSQSEndpoint(context =>
    {
        var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("Samples.DynamoDB.Lambda.Sales");

        var advanced = endpointConfiguration.AdvancedConfiguration;
        advanced.UseSerialization<SystemJsonSerializer>();
        advanced.SendFailedMessagesTo("Samples-DynamoDB-Lambda-Error");

        advanced.CustomDiagnosticsWriter((diagnostics,_) =>
        {
            context.Logger.LogLine(diagnostics);
            return Task.CompletedTask;
        });

        var connection =
            "server=localhost;user=root;database=dbname;port=3306;password=pass;AllowUserVariables=True;AutoEnlist=false";
        var persistence = advanced.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        persistence.SqlDialect<SqlDialect.MySql>();
        persistence.ConnectionBuilder(
            connectionBuilder: () => new MySqlConnection(connection));

        return endpointConfiguration;
    });

    #endregion

    #region FunctionHandler

    public async Task ProcessOrder(SQSEvent eventData, ILambdaContext context)
    {
        context.Logger.Log("ProcessOrder was called");

        await endpoint.Process(eventData, context, CancellationToken.None);
    }

    #endregion
}