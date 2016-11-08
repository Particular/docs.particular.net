using System;
using System.Security.Principal;
using System.Threading;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint1";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.UsernameHeader.Endpoint1");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();
        #region ComponentRegistration
        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<UsernameMutator>(DependencyLifecycle.InstancePerCall);
        });
        #endregion
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            #region SendMessage

            var identity = new GenericIdentity("FakeUser");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);
            bus.Send("Samples.UsernameHeader.Endpoint2", new MyMessage());
            #endregion
            Console.WriteLine("Message sent. Press any key to exit");
            Console.ReadKey();
        }
    }
}