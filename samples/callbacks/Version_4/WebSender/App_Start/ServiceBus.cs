namespace WebSender
{
    using NServiceBus;
    using NServiceBus.Installation.Environments;

    public class ServiceBus
    {
        public static IBus Bus { get; private set; }

        static readonly object padlock = new object();

        public static void Init()
        {
            if (Bus != null)
                return;

            lock (padlock)
            {
                if (Bus != null)
                    return;

                Configure.Serialization.Json();
                Configure configure = Configure.With();
                configure.Log4Net();
                configure.DefineEndpointName("Samples.Callbacks.WebSender");
                configure.DefaultBuilder();
                configure.InMemorySagaPersister();
                configure.UseInMemoryTimeoutPersister();
                configure.InMemorySubscriptionStorage();
                configure.UseTransport<Msmq>();
                Bus = configure.UnicastBus().CreateBus().Start(() => configure.ForInstallationOn<Windows>().Install());
            }
        }
    }
}