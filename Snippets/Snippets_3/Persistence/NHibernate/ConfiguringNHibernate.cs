using NServiceBus;

class ConfiguringNHibernate
{
    public void Foo()
    {
        #region ConfiguringNHibernateV3

        Configure.With()
                    .DefaultBuilder()
                    .NHibernateSagaPersister()
                    .UseNHibernateTimeoutPersister()
                    .DBSubcriptionStorage();

        #endregion
    }
}