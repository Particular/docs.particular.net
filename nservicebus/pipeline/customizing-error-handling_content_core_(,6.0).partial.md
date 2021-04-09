`IManageMessageFailures` allows custom actions when messages continue to fail after the [Immediate Retries](/nservicebus/recoverability/#immediate-retries) have been attempted.

WARNING: When enabling this extension, second-level retries will not be invoked. Versions 6 and above offer better control of customization through the message pipeline.

snippet: CustomFaultManager

This extension must be registered in dependency injection so that it can be invoked when failures occur. Registration of this component can be done using the `INeedInitialization` interface. See also [dependency-injection child lifetime](/nservicebus/dependency-injection/child-lifetime.md).

snippet: RegisterFaultManager
