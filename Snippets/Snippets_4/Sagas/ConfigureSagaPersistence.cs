namespace Snippets4.Sagas
{
    using NServiceBus;
    using NServiceBus.Features;

    class ConfigureSagaPersistence
    {

        ConfigureSagaPersistence(Configure configure)
        {
            #region saga-configure

            Feature.Enable<Sagas>();
            configure.DefaultBuilder();
            configure.UseTransport<Msmq>();
            configure.UnicastBus();
            configure.RavenSagaPersister();

            #endregion
        }

    }
}