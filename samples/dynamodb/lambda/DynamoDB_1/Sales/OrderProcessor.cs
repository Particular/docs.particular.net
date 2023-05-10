using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using NServiceBus;

namespace Sales
{
  public class OrderProcessor
  {
    #region EndpointSetup

    static readonly AwsLambdaSQSEndpoint endpoint = new AwsLambdaSQSEndpoint(context =>
    {
      var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("Samples.DynamoDB.Lambda.Sales");


      var advanced = endpointConfiguration.AdvancedConfiguration;
      advanced.SendFailedMessagesTo("Samples.DynamoDB.Lambda.Error");
      advanced.EnableInstallers();

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
}