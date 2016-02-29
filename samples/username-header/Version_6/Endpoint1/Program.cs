using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint1";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.UsernameHeader.Endpoint1");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.SendFailedMessagesTo("error");
        
        #region ComponentRegistartion
        endpointConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<UsernameMutator>(DependencyLifecycle.InstancePerCall);
        });
        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            #region SendMessage
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("FakeUser"), new string[0]);
            await endpoint.Send("Samples.UsernameHeader.Endpoint2", new MyMessage());
            #endregion
            Console.WriteLine("Message sent. Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}