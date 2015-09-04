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

                Configure configure = Configure.With();
                configure.Log4Net();
                configure.DefineEndpointName("Samples.Callbacks.WebSender");
                configure.DefaultBuilder();
                configure.MsmqTransport();
                configure.InMemorySagaPersister();
                configure.UseInMemoryTimeoutPersister();
                configure.RunTimeoutManagerWithInMemoryPersistence();
                configure.InMemorySubscriptionStorage();
                configure.JsonSerializer();
                Bus = configure.UnicastBus().CreateBus().Start(() => configure.ForInstallationOn<Windows>().Install());
            }
        }
    }
}