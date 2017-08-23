It is possible to create a custom DataBus implementation. This is done by making use of the [Features](/nservicebus/pipeline/features.md) extension.


## Implement the `IDataBus` interface. 

This new class will provide the custom implementations for the `Get` and `Put` methods for the DataBus.

snippet: CustomDataBus

This new implementation needs to be registered as a new feature. 


## Define a feature 

Define a [new feature](/nservicebus/pipeline/features.md) that registers the custom DataBus implementation class.

snippet: CustomDataBusFeature


## Define a DataBusDefinition

Define a new class which inherits from the `DataBusDefinition` class.

snippet: CustomDataBusDefinition


## Configure the endpoint

Configure the endpoint to use the custom DataBus implementation instead of the default DataBus:

snippet: PluginCustomDataBus
