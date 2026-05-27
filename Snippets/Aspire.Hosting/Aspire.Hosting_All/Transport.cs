using Aspire.Hosting;
using Particular.Aspire.Hosting.ServicePlatform.Transport;

namespace Projects;

public class Transport
{
    public void AppHost(DistributedApplicationBuilder builder)
    {
        #region aspire-transport-learning

        builder
            .AddParticularPlatform("particular")
            .WithTransportLearning();

        #endregion

        #region aspire-transport-asb

        var asb = builder.AddConnectionString("asb");
        // var asb = builder.AddAzureServiceBus("asb");

        builder
            .AddParticularPlatform("particular")
            .WithTransportAzureServiceBus(asb);

        #endregion

        #region aspire-transport-rabbitmq

        var rabbit = builder.AddConnectionString("rabbit");
        // var rabbit = builder.AddRabbitMQ("rabbit");

        builder
            .AddParticularPlatform("particular")
            .WithTransportRabbitMQ(RabbitMqRouting.QuorumConventionalRouting, rabbit);

        #endregion

        #region aspire-transport-sqs

        var sqsAccessId = builder.AddParameter("sqs-access-id");
        var sqsSecretKey = builder.AddParameter("sqs-secret-key");

        builder
            .AddParticularPlatform("particular")
            .WithTransportAmazonSqs("us-west-1", sqsAccessId.Resource, sqsSecretKey.Resource);

        #endregion
    }
}