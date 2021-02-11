using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Usage

        var transport = new SqlServerTransport("connectionString");
        endpointConfiguration.UseTransport(transport);

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