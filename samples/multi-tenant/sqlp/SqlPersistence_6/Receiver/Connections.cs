using System;

public static class Connections
{
    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SqlMultiTenantA;Integrated Security=True;Max Pool Size=100;Encrypt=false
    public static string TenantA = @"Server=localhost,1433;Initial Catalog=SqlMultiTenantA;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SqlMultiTenantA;Integrated Security=True;Max Pool Size=100;Encrypt=false
    public static string TenantB = @"Server=localhost,1433;Initial Catalog=SqlMultiTenantB;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

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