using System;

public static class Connections
{
    static Connections()
    {
        SqlHelper.EnsureDatabaseExists(Default);
        SqlHelper.EnsureDatabaseExists(TenantA);
        SqlHelper.EnsureDatabaseExists(TenantB);
    }
    public static string Default = @"Data Source=.\SqlExpress;Database=NsbSamplesMultiTenantReceiver;Integrated Security=True";
    public static string TenantA = @"Data Source=.\SqlExpress;Database=NsbSamplesMultiTenantA;Integrated Security=True";
    public static string TenantB = @"Data Source=.\SqlExpress;Database=NsbSamplesMultiTenantB;Integrated Security=True";

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