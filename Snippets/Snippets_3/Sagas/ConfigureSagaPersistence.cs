namespace Snippets3.Sagas
{
    using NServiceBus;

    public class ConfigureSagaPersistence
    {

        public void Simple()
        {
            #region saga-configure

            Configure configure = Configure.With();
            configure.DefaultBuilder();
            configure.MsmqTransport();
            configure.Sagas();
            configure.UnicastBus();
            configure.RavenSagaPersister();
            IStartableBus bus = configure.CreateBus();

            #endregion
        }
    }
}
