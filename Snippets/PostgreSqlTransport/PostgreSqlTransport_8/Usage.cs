using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Usage

        endpointConfiguration.UseTransport(new PostgreSqlTransport("connectionString"));

        #endregion

    }

    void MessageBodyStringConfiguration()
    {
        #region MessageBodyString-config

        var transport = new PostgreSqlTransport("connectionString")
        {
            CreateMessageBodyComputedColumn = true
        };

        #endregion
    }
}