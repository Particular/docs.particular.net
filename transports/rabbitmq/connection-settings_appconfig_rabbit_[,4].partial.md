### Specifying the connection string via app.config

By default, the transport will look for a connection string called `NServiceBus/Transport` in `app.config`:

snippet: rabbitmq-connectionstring

To use a custom name for the connection string:

snippet: rabbitmq-config-connectionstringname