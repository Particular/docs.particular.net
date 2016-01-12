using System;
using System.Security.Principal;
using System.Threading;
using NServiceBus;

class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.UsernameHeader.Endpoint1");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();
        #region ComponentRegistartion
        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<UsernameMutator>(DependencyLifecycle.InstancePerCall);
        });
        #endregion
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            #region SendMessage
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("FakeUser"), new string[0]);
            bus.Send("Samples.UsernameHeader.Endpoint2", new MyMessage());
            #endregion
            Console.WriteLine("Message sent. Press any key to exit");
            Console.ReadKey();
        }
    }
}