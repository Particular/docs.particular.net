using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using MySqlConnector;

public class OrderProcessor
{
    #region EndpointSetup

    static readonly AwsLambdaSQSEndpoint endpoint = new AwsLambdaSQSEndpoint(context =>
    {
        var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("Samples.Aurora.Lambda.Sales");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var connection = Environment.GetEnvironmentVariable("AuroraLambda_ConnectionString");

        var persistence = endpointConfiguration.AdvancedConfiguration.UsePersistence<SqlPersistence>();
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