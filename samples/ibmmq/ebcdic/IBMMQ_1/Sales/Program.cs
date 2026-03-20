using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.IBMMQ;

var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(context =>
    {
        var endpointConfiguration = new EndpointConfiguration("Sales");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();


        var transport = new IBMMQTransport(o =>
        {
            o.Host = "ibmmq";
            o.Port = 1414;
            o.QueueManagerName = "QM1";
            o.Channel = "APP.SVRCONN";
            o.User = "sales";
        });

        var routing = endpointConfiguration.UseTransport(transport);
        endpointConfiguration.EnableInstallers();


        endpointConfiguration.EnableFeature<EbcdicEnvelopeFeature>();

        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");

        return endpointConfiguration;
    })
    .Build();

await host.RunAsync();
