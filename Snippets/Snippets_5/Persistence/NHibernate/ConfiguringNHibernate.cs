using NServiceBus;
using NServiceBus.Persistence;

class ConfiguringNHibernate
{
    public void Foo()
    {
        #region ConfiguringNHibernate

        var config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence>()
            .For(
                Storage.Sagas,
                Storage.Subscriptions,
                Storage.Timeouts,
                Storage.Outbox,
                Storage.GatewayDeduplication);

        #endregion
    }
}