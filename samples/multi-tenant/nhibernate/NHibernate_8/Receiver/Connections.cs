using System;

public static class Connections
{
    static Connections()
    {
        SqlHelper.EnsureDatabaseExists(Shared);
        SqlHelper.EnsureDatabaseExists(TenantA);
        SqlHelper.EnsureDatabaseExists(TenantB);
    }
    public static string Shared = @"Data Source=.\SqlExpress;Database=NHibernateMultiTenantReceiver;Integrated Security=True";
    public static string TenantA = @"Data Source=.\SqlExpress;Database=NHibernateMultiTenantA;Integrated Security=True";
    public static string TenantB = @"Data Source=.\SqlExpress;Database=NHibernateMultiTenantB;Integrated Security=True";

    public static string GetTenant(string id)
    {
        if (id == "A")
        {
            return TenantA;
        }
        if (id == "B")
        {
            return TenantB;
        }
        throw new Exception($"Invalid tenant '{id}'.");
    }
}