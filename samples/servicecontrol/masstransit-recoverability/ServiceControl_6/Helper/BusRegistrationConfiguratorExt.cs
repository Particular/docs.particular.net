using System.Reflection.Metadata;
using MassTransit;

public static class BusRegistrationConfiguratorExt
{
    public static void SetupTransport(this IBusRegistrationConfigurator x, string[] args)
    {
        string selectedTransport;

        if (args.Contains("--amazonsqs"))
        {
            x.UsingAmazonSqs((ctx, cfg) =>
            {
                cfg.Host(Environment.GetEnvironmentVariable("AWS_REGION"), h =>
                {
                    h.AccessKey(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"));
                    h.SecretKey(Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"));
                });

                cfg.ConfigureEndpoints(ctx);
            });
            selectedTransport = "AmazonSQS";
        }
        else if (args.Contains("--azureservicebus"))
        {

            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(Environment.GetEnvironmentVariable("CONNECTIONSTRING_AZURESERVICEBUS"));

                cfg.ConfigureEndpoints(context);
            });

            selectedTransport = "Azure Service Bus";
        }
        else if (args.Contains("--rabbitmq"))
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", 56721, "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });

            selectedTransport = "RabbitMQ";
        }
        else
        {
            throw new ArgumentException("No transport is chosen");
        }

        var name = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
        Console.Title = name + " - " + selectedTransport;

        x.AddConfigureEndpointsCallback((name, cfg) =>
        {
            if (cfg is IRabbitMqReceiveEndpointConfigurator rmq)
            {
                rmq.SetQuorumQueue();
            }

            if (cfg is IServiceBusReceiveEndpointConfigurator sb)
            {
                sb.ConfigureDeadLetterQueueDeadLetterTransport();
                sb.ConfigureDeadLetterQueueErrorTransport();
            }
        });
    }
}
