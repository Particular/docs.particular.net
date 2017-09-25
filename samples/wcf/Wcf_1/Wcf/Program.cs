using System;
using System.ServiceModel;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Wcf.Endpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.CustomRouting.Client");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        #region enable-wcf

        endpointConfiguration.MakeInstanceUniquelyAddressable("1");
        endpointConfiguration.EnableCallbacks();

        var wcf = endpointConfiguration.Wcf();
        wcf.Binding(
            provider: type =>
            {
                return new BindingConfiguration(
                    binding: new NetNamedPipeBinding(),
                    address: new Uri("net.pipe://localhost/MyService"));
            });
        wcf.CancelAfter(
            provider: type =>
            {
                return type.IsAssignableFrom(typeof(MyService))
                    ? TimeSpan.FromSeconds(5)
                    : TimeSpan.FromSeconds(60);
            });

        #endregion

        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press <enter> to send a message");
        Console.WriteLine("Press <escape> to send a message which will time out");
        Console.WriteLine("Press any key to exit");

        #region wcf-proxy

        var pipeFactory = new ChannelFactory<IWcfService<MyRequestMessage, MyResponseMessage>>(
            binding: new NetNamedPipeBinding(),
            remoteAddress: new EndpointAddress("net.pipe://localhost/MyService"));
        var pipeProxy = pipeFactory.CreateChannel();

        #endregion

        Console.WriteLine("Proxy initialized.");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (!(key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Escape))
            {
                break;
            }

            try
            {
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("Sending request that will succeed over proxy.");
                }

                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Sending request that will fail over proxy.");
                }

                #region wcf-proxy-call

                var request = new MyRequestMessage
                {
                    Info = key.Key == ConsoleKey.Enter
                        ? "Hello from handler"
                        : "Cancel"
                };
                var response = await pipeProxy.Process(request)
                    .ConfigureAwait(false);

                #endregion

                Console.WriteLine($"Response '{response.Info}'");
            }
            catch (FaultException faultException)
            {
                Console.Error.WriteLine($"Request failed due to: '{faultException.Message}'");
            }
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}