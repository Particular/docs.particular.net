using System;

public static class Connections
{
    static Connections()
    {
        SqlHelper.EnsureDatabaseExists(TenantA);
        SqlHelper.EnsureDatabaseExists(TenantB);
    }

    public static string TenantA = @"Data Source=.\SqlExpress;Database=SqlMultiTenantA;Integrated Security=True";
    public static string TenantB = @"Data Source=.\SqlExpress;Database=SqlMultiTenantB;Integrated Security=True";

    public static string GetForTenant(string id)
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