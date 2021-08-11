NServiceBus can be registered and configured on the host builder using the `UseNServiceBus` extension method in the startup class:

snippet: asb-function-default

The default builder method will look up values such as function name, connection string and license information directly from from the `IConfiguration`. Fine-grain configuration is also possible.

snippet: asb-function-hostbuilder