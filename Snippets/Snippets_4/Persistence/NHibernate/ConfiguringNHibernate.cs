using NServiceBus;

class ConfiguringNHibernate
{
    public void Foo()
    {
        #region ConfiguringNHibernate

        Configure.With()
        .UseNHibernateSubscriptionPersister()
        .UseNHibernateTimeoutPersister() 
        .UseNHibernateSagaPersister() 
        .UseNHibernateGatewayPersister();

        #endregion
    }
}