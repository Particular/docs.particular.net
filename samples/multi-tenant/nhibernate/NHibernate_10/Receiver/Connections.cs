using System;

public static class Connections
{
    static Connections()
    {
        SqlHelper.EnsureDatabaseExists(Shared);
        SqlHelper.EnsureDatabaseExists(TenantA);
        SqlHelper.EnsureDatabaseExists(TenantB);
    }

    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NHibernateMultiTenantReceiver;Integrated Security=True;Encrypt=false
    public const string Shared = @"Server=localhost,1433;Initial Catalog=NHibernateMultiTenantReceiver;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";
    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NHibernateMultiTenantA;Integrated Security=True;Encrypt=false
    public const string TenantA = @"Server=localhost,1433;Initial Catalog=NHibernateMultiTenantA;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";
    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NHibernateMultiTenantB;Integrated Security=True;Encrypt=false
    public const string TenantB = @"Server=localhost,1433;Initial Catalog=NHibernateMultiTenantB;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

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