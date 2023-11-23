using Amazon.SimpleNotificationService;
using Amazon.SQS;
using NServiceBus;

class Upgrade
{
    void Clients(EndpointConfiguration endpointConfiguration)
    {
        #region 5to6-clients

        var transport = new SqsTransport(
            new AmazonSQSClient(), 
            new AmazonSimpleNotificationServiceClient());

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}