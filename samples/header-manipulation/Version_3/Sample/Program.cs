using System;
using System.Collections.Generic;
using System.Threading;
using NServiceBus;
using NServiceBus.Installation.Environments;
using NServiceBus.ObjectBuilder;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Headers";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Headers");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        IConfigureComponents components = configure.Configurer;
        components.ConfigureComponent<MutateIncomingMessages>(DependencyLifecycle.InstancePerCall);
        components.ConfigureComponent<MutateIncomingTransportMessages>(DependencyLifecycle.InstancePerCall);
        components.ConfigureComponent<MutateOutgoingMessages>(DependencyLifecycle.InstancePerCall);
        components.ConfigureComponent<MutateOutgoingTransportMessages>(DependencyLifecycle.InstancePerCall);
        components.ConfigureComponent<MessageMutator>(DependencyLifecycle.InstancePerCall);
        components.ConfigureComponent<MutateTransportMessages>(DependencyLifecycle.InstancePerCall);

        #region global-all-outgoing

        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IDictionary<string, string> outgoingHeaders = ((IBus) startableBus).OutgoingHeaders;
            outgoingHeaders.Add("AllOutgoing", "ValueAllOutgoing");

            #endregion

            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

            #region sending
            MyMessage myMessage = new MyMessage();
            myMessage.SetHeader("SendingMessage", "ValueSendingMessage");
            bus.SendLocal(myMessage);
            #endregion

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}