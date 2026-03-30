using System.Text;
using Microsoft.Extensions.Hosting;
using NServiceBus.MessageMutator;
using NServiceBus.Transport.IBMMQ;

var host = Host
    .CreateDefaultBuilder(args)
    .UseNServiceBus(context =>
    {
        var endpointConfiguration = new EndpointConfiguration("DEV.RECEIVER");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.Recoverability().Delayed(settings => settings.NumberOfRetries(0));
        endpointConfiguration.EnableInstallers();

        var transport = new IBMMQTransport()
        {
            QueueManagerName = "QM1",
            Channel = "DEV.ADMIN.SVRCONN",
            User = "admin",
            Password = "passw0rd"
        };
        endpointConfiguration.UseTransport(transport);

        #region FixedLengthEBCDICToJsonMutatorRegistration
        //in order to use IBM500 EBCDIC encoding, we need to register the code page provider
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        endpointConfiguration.RegisterMessageMutator(new FixedLengthEBCDICToJsonMutator());

        #endregion
      
        return endpointConfiguration;
    })
    .Build();

await host.RunAsync();
