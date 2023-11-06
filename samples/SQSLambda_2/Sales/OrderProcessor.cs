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
        var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("Samples.Aurora.Lambda.Sales");

        var advanced = endpointConfiguration.AdvancedConfiguration;
        advanced.UseSerialization<SystemJsonSerializer>();
        advanced.SendFailedMessagesTo("Samples-Aurora-Lambda-Error");

        var connection = Environment.GetEnvironmentVariable("AuroraLambda_ConnectionString");

        var persistence = advanced.UsePersistence<SqlPersistence>();
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