It is possible to create a custom DataBus implementation. This is done by making use of the [Features](/nservicebus/pipeline/features.md) extension.

Implement the `IDataBus` interface. This new class will provide the custom implementations for the `Get` and `Put` methods for the DataBus. 

snippet: CustomDataBus

This new implementation needs to be now registered as a new feature. To do this, define a [new feature](/nservicebus/pipeline/features.md) for the custom databus by creating a class that inherits from `Feature`. In the Setup method, register the custom DataBus implementation class.

snippet: CustomDataBusFeature

Now define a new class which inherits from the `DataBusDefinition` class. This class will be used at endpoint configuration time, to instruct NServiceBus to utilize the custom DataBus instead of the default DataBus. 

snippet: CustomDataBusDefinition

When customizing DataBus, it may be desirable to also [customize the DataBus serializer](/samples/databus/custom-serializer/) to use Json instead of binary serialization.

