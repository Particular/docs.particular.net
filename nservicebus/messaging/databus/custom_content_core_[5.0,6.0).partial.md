It is possible to create a custom DataBus implementation by implementing the `IDataBus` interface, such as in the following minimalistic sample:

snippet: CustomDataBus

To configure the endpoint to use the custom DataBus implementation it is enough to register it at endpoint configuration time, as shown:

snippet: PluginCustomDataBus
