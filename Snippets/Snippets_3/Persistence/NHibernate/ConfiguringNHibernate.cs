using NServiceBus;

class ConfiguringNHibernate
{
    public void Foo()
    {
        #region ConfiguringNHibernate

        Configure.With()
                    .DefaultBuilder()
                    .NHibernateSagaPersister()
                    .UseNHibernateTimeoutPersister()
                    .DBSubcriptionStorage();

        #endregion
    }
}