using NServiceBus;

class ConfiguringNHibernate
{
    public void Foo()
    {
        #region ConfiguringNHibernateV4

        Configure.With()
        .UseNHibernateSubscriptionPersister()
        .UseNHibernateTimeoutPersister() 
        .UseNHibernateSagaPersister() 
        .UseNHibernateGatewayPersister();

        #endregion
    }
}