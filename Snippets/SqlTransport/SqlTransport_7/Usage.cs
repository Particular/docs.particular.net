using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Usage

        endpointConfiguration.UseTransport(new SqlServerTransport("connectionString"));

        #endregion
    }

    void MessageBodyStringConfiguration()
    {
        #region MessageBodyString-config

        var transport = new SqlServerTransport("connectionString")
        {
            CreateMessageBodyComputedColumn = true
        };

        #endregion
    }
}