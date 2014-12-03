using NServiceBus;

class ConfiguringNHibernate
{
    public void Foo()
    {
        #region ConfiguringNHibernate 3

        Configure.With()
                    .DefaultBuilder()
                    .NHibernateSagaPersister()
                    .UseNHibernateTimeoutPersister()
                    .DBSubcriptionStorage();

        #endregion
    }
}