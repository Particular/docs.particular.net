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

        builder
            .AddParticularPlatform("particular")
            .WithTransportAzureServiceBus(asb);

        #endregion

        #region aspire-transport-rabbitmq

        var rabbit = builder.AddConnectionString("rabbit");

        builder
            .AddParticularPlatform("particular")
            .WithTransportRabbitMQ(RabbitMqRouting.QuorumConventionalRouting, rabbit);

        #endregion
    }
}