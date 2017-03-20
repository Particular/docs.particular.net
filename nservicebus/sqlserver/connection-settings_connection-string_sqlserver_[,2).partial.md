### Via the App.Config

By adding a connection named `NServiceBus/Transport` in the `connectionStrings` node.

snippet: sqlserver-connection-string-xml


### Via a named connection string

By using the `ConnectionStringName` extension method:

snippet: sqlserver-named-connection-string

Combined this with a named connection in the `connectionStrings` node of the `app.config` file:

snippet: sqlserver-named-connection-string-xml
