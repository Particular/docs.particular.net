using System;

public static class Connections
{
    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SqlMultiTenantA;Integrated Security=True;Max Pool Size=100;Encrypt=false
    public const string TenantA = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SqlMultiTenantA;Integrated Security=True;Max Pool Size=100;Encrypt=false";
    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SqlMultiTenantA;Integrated Security=True;Max Pool Size=100;Encrypt=false
    public const string TenantB = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SqlMultiTenantA;Integrated Security=True;Max Pool Size=100;Encrypt=false";

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