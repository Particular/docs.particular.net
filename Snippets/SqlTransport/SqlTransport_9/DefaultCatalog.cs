using NServiceBus;

class DefaultCatalog
{
    void OverwriteDefaultCatalog()
    {
        #region sqlserver-default-catalog

        var transport = new SqlServerTransport("connectionString")
        {
            DefaultCatalog = "mycatalog"
        };

        #endregion
    }
}