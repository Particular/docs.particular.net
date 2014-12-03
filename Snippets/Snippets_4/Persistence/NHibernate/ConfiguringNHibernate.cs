using NServiceBus;

class ConfiguringNHibernate
{
    public void Foo()
    {
        #region ConfiguringNHibernate 4

        Configure.With()
        .UseNHibernateSubscriptionPersister()
        .UseNHibernateTimeoutPersister() 
        .UseNHibernateSagaPersister() 
        .UseNHibernateGatewayPersister();

        #endregion
    }
}