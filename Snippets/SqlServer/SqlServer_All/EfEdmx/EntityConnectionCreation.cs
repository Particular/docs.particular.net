using System.Data.Entity.Core.EntityClient;
using SqlServer_All.EfEdmx;

class EntityConnectionCreation
{
    void Temp()
    {
        #region EntityConnectionCreationAndUsage

        EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder
        {
            Provider = "System.Data.SqlClient",
            ProviderConnectionString = "the database connection string",
            Metadata = "res://*/MySample.csdl|res://*/MySample.ssdl|res://*/MySample.msl"
        };

        EntityConnection entityConn = new EntityConnection(entityBuilder.ToString());

        using (MySampleContainer ctx = new MySampleContainer(entityConn))
        {
            //use the DbContext as required
        }

        #endregion
    }
}
