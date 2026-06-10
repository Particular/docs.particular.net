using Aspire.Hosting;

public class Persistence
{
    public void Managed(DistributedApplicationBuilder builder)
    {
        #region aspire-persistence-ravendb-managed

        var platform = builder.AddParticularPlatform("particular");

        var serviceControlDb = platform.AddPersistenceRavenDb("ravendb");

        #endregion
    }

    public void External(DistributedApplicationBuilder builder)
    {
        #region aspire-persistence-ravendb-external

        var ravenDb = builder.AddConnectionString("ravendb");

        builder
            .AddParticularPlatform("particular")
            .WithPersistenceRavenDb(ravenDb);

        #endregion
    }
}
