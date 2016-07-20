using System.Data.Entity.Core.EntityClient;
using SqlServer_All.EfEdmx;

class EntityConnectionCreation
{
    void Temp()
    {
        #region EntityConnectionCreationAndUsage

        var entityBuilder = new EntityConnectionStringBuilder
        {
            Provider = "System.Data.SqlClient",
            ProviderConnectionString = "the database connection string",
            Metadata = "res://*/MySample.csdl|res://*/MySample.ssdl|res://*/MySample.msl"
        };

        var entityConn = new EntityConnection(entityBuilder.ToString());

        using (var mySampleContainer = new MySampleContainer(entityConn))
        {
            // use the DbContext as required
        }

        #endregion
    }
}
