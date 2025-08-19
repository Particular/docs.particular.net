using System;
using System.ServiceModel;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "WcfEndpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Wcf.Endpoint");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.EnableInstallers();

        #region enable-wcf

        endpointConfiguration.MakeInstanceUniquelyAddressable("1");
        endpointConfiguration.EnableCallbacks();

        var wcf = endpointConfiguration.Wcf();
        wcf.Binding(
            provider: _ => new BindingConfiguration(
                binding: new NetNamedPipeBinding(),
                address: new Uri("net.pipe://localhost/MyService")));
        wcf.CancelAfter(
            provider: type => type.IsAssignableFrom(typeof(MyService))
                ? TimeSpan.FromSeconds(5)
                : TimeSpan.FromSeconds(60));

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await Console.Out.WriteLineAsync("Press <enter> to send a message");
        await Console.Out.WriteLineAsync("Press <escape> to send a message which will time out");
        await Console.Out.WriteLineAsync("Press any key to exit");

        #region wcf-proxy

        var pipeFactory = new ChannelFactory<IWcfService<MyRequestMessage, MyResponseMessage>>(
            binding: new NetNamedPipeBinding(),
            remoteAddress: new EndpointAddress("net.pipe://localhost/MyService"));
        var pipeProxy = pipeFactory.CreateChannel();

        #endregion

        await Console.Out.WriteLineAsync("Proxy initialized.");

        while (true)
        {
            var key = Console.ReadKey();
            await Console.Out.WriteLineAsync();

            if (key.Key is not (ConsoleKey.Enter or ConsoleKey.Escape))
            {
                break;
            }

            try
            {
                if (key.Key == ConsoleKey.Enter)
                {
                    await Console.Out.WriteLineAsync("Sending request that will succeed over proxy.");
                }

                if (key.Key == ConsoleKey.Escape)
                {
                    await Console.Out.WriteLineAsync("Sending request that will fail over proxy.");
                }

                #region wcf-proxy-call

                var request = new MyRequestMessage
                {
                    Info = key.Key == ConsoleKey.Enter ? "Hello from handler" : "Cancel"
                };
                var response = await pipeProxy.Process(request);

                #endregion

                await Console.Out.WriteLineAsync($"Response '{response.Info}'");
            }
            catch (FaultException faultException)
            {
                await Console.Error.WriteLineAsync($"Request failed due to: '{faultException.Message}'");
            }
        }
        await endpointInstance.Stop();
    }
}
