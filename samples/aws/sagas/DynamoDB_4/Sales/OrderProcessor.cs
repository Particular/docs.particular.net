using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

public class OrderProcessor
{
    #region EndpointSetup

    static readonly AwsLambdaSQSEndpoint endpoint = new AwsLambdaSQSEndpoint(context =>
    {
        var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("Samples.DynamoDB.Lambda.Sales");

        var advanced = endpointConfiguration.AdvancedConfiguration;
        advanced.UseSerialization<SystemJsonSerializer>();
        advanced.SendFailedMessagesTo("Samples-DynamoDB-Lambda-Error");

        var persistence = advanced.UsePersistence<DynamoPersistence>();

        persistence.UseSharedTable(new TableConfiguration()
        {
            TableName = "Samples.DynamoDB.Lambda",
        });

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
