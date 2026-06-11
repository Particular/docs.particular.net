Each of the endpoint projects contain the same code to create an application host, apply the configuration from the ServiceDefaults project on the NServiceBus endpoint.

snippet: endpoint-config

The service defaults extension method retrieves the connection string for the RabbitMQ broker and configures NServiceBus to use it as a transport:

snippet: transport-config

The Shipping endpoint additionally retrieves the connection string for the PostgreSQL database and configures NServiceBus to use it as a ServiceControl database:

snippet: persistence-config

Additionally, the shared config enables NServiceBus installers. Every time the application host is run, the transport and ServiceControl database are recreated and will not contain the queues and tables needed for the endpoints to run. Enabling installers allows NServiceBus to set up the assets that it needs at runtime.

snippet: enable-installers

If you're missing certain capabilities to use Aspire with NServiceBus, [share them and help shape the future of the platform](/shape-the-future/aspire.md).