public static class Connections
{
    static Connections()
    {
        SqlHelper.EnsureDatabaseExists(Shared);
        SqlHelper.EnsureDatabaseExists(TenantA);
        SqlHelper.EnsureDatabaseExists(TenantB);
    }

    // For SQL Server Express use:         Data Source=.\SqlExpress;Integrated Security=True;Encrypt=false;Max Pool Size=100
    // For SQL Server Express LocalDb use: Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Encrypt=false;Max Pool Size=100
    private const string Base = @"Data Source=.;User Id=SA;Password=yourStrong(!)Password;Encrypt=false;Max Pool Size=100";

    public static readonly string Shared = $"{Base};Initial Catalog=NHibernateMultiTenantReceiver";
    public static readonly string TenantA = $"{Base};Initial Catalog=NHibernateMultiTenantA";
    public static readonly string TenantB = $"{Base};Initial Catalog=NHibernateMultiTenantB";

    public static string GetForTenant(string id) => id switch
    {
        "A" => TenantA,
        "B" => TenantB,
        _ => throw new ArgumentException($"Invalid tenant ID '{id}'.", nameof(id))
    };
}