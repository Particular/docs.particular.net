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
        wcf.Binding(t => new BindingConfiguration(new NetNamedPipeBinding(), new Uri("net.pipe://localhost/MyService")));
        wcf.CancelAfter(t => t.IsAssignableFrom(typeof(MyService)) ? TimeSpan.FromSeconds(5) : TimeSpan.FromSeconds(60));
        #endregion

        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press <enter> to send a message");
        Console.WriteLine("Press <escape> to send a message which will time out");
        Console.WriteLine("Press any key to exit");

        #region wcf-proxy
        var pipeFactory = new ChannelFactory<IWcfService<MyRequestMessage, MyResponseMessage>>(new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/MyService"));
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
                var response = await pipeProxy.Process(new MyRequestMessage
                    {
                        Info = key.Key == ConsoleKey.Enter ? "Hello from handler" : "Cancel"
                    }).ConfigureAwait(false);
                #endregion

                Console.WriteLine($"Response '{response.Info}'");
            }
            catch (FaultException ex)
            {
                Console.Error.WriteLine($"Request failed due to: '{ex.Message}'");
            }
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}

public class MyService : WcfService<MyRequestMessage, MyResponseMessage>
{
}

#region wcf-reply-handler
public class MyRequestMessageHandler : IHandleMessages<MyRequestMessage>
{
    public Task Handle(MyRequestMessage message, IMessageHandlerContext context)
    {
        if (message.Info == "Cancel")
        {
            return Task.CompletedTask;
        }
        return context.Reply(new MyResponseMessage { Info = message.Info });
    }
}
#endregion

public class MyRequestMessage : ICommand
{
    public string Info { get; set; }

}

public class MyResponseMessage : IMessage
{
    public string Info { get; set; }
}