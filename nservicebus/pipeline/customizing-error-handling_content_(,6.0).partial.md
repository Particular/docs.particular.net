`IManageMessageFailures` allows custom actions when messages continue to fail after the [Immediate Retries](/nservicebus/recoverability/#immediate-retries) have been attempted.

WARNING: When enabling this extension, second-level retries will not be invoked. Versions 6 and above offer better control of customization through the message pipeline.

snippet: CustomFaultManager

This extension needs to be registered in the container so that it can be invoked when the failures occur. Registration of this component can be done using the `INeedInitialization` interface. Read this article for more details on [how to register custom components](/nservicebus/containers/child-containers.md) in the container.

snippet: RegisterFaultManager
