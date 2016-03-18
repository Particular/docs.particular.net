using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncRun().GetAwaiter().GetResult();
    }

    static async Task AsyncRun()
    {
        Console.Title = "Samples.Headers";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.Headers");

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<MutateIncomingMessages>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<MutateIncomingTransportMessages>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<MutateOutgoingMessages>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<MutateOutgoingTransportMessages>(DependencyLifecycle.InstancePerCall);
        });

        #region global-all-outgoing

        endpointConfiguration.AddHeaderToAllOutgoingMessages("AllOutgoing", "ValueAllOutgoing");

        #endregion
        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            #region sending

            MyMessage myMessage = new MyMessage();
            await endpoint.SendLocal(myMessage);

            #endregion

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}