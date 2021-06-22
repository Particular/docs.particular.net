using NServiceBus;

namespace SqlTransport_6._3
{
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
}
