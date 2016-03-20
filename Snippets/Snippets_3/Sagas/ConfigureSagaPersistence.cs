namespace Snippets3.Sagas
{
    using NServiceBus;

    class ConfigureSagaPersistence
    {

        ConfigureSagaPersistence(Configure configure)
        {
            #region saga-configure

            configure.Sagas();
            configure.RavenSagaPersister();

            #endregion
        }
    }
}