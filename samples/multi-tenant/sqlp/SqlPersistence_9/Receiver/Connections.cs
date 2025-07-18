public static class Connections
{
    // For SQL Server Express use:         Data Source=.\SqlExpress;Integrated Security=True;Encrypt=false;Max Pool Size=100
    // For SQL Server Express LocalDb use: Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Encrypt=false;Max Pool Size=100
    private const string Base = @"Data Source=.;User Id=SA;Password=yourStrong(!)Password;Encrypt=false;Max Pool Size=100";

    public static readonly string TenantA = $"{Base};Initial Catalog=SqlMultiTenantA";
    public static readonly string TenantB = $"{Base};Initial Catalog=SqlMultiTenantB";

    public static string GetForTenant(string id) => id switch
    {
        "A" => TenantA,
        "B" => TenantB,
        _ => throw new ArgumentException($"Invalid tenant '{id}'.", nameof(id))
    };
}