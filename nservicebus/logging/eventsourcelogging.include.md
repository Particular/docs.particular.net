## Required Implementations

Several class implementations are required to define how the EventSourceLogger should function.


### EventListener

The implementation of [EventListener](https://msdn.microsoft.com/en-us/library/system.diagnostics.tracing.eventlistener.aspx) gives full contol over how [EventWrittenEventArgs](https://msdn.microsoft.com/en-us/library/system.diagnostics.tracing.eventlistener.oneventwritten.aspx) are written.

In this case only [OnEventWritten](https://msdn.microsoft.com/en-us/library/system.diagnostics.tracing.eventlistener.oneventwritten.aspx) is overwritten to write to the console.

> OnEventWritten is called whenever an event has been written by an event source for which the event listener has enabled events.

Typically, events would not be written to the console. This is for example purposes only.

snippet: EventListener


### EventSourceLoggerBase

The implementation of `EventSourceLoggerBase` allows custom handling of of all levels of logging as well as assigning the [EventAttribute](https://msdn.microsoft.com/en-us/library/system.diagnostics.tracing.eventattribute.aspx) settings that will be used for each method.

snippet: EventSourceLoggerBase


## Enabling Logging

Enabling EventSourceLogger involves several steps

 1. Instantiate the `EventListener`.
 1. Instantiate the `EventSourceLoggerBase`.
 1. Pass the `EventSourceLoggerBase` instance to the `EventListener` instance with an [EventLevel](https://msdn.microsoft.com/en-us/library/system.diagnostics.tracing.eventlevel.aspx).
 1. Configure NServiceBus to use EventSourceLogger by calling `LogManager.Use<EventSourceLoggingFactory>()` and then passing the `EventSourceLoggerBase` instance to the `EventSourceLoggingFactory`.

Then at service shutdown time dispose of both the instance of `EventListener` and the instance of `EventSourceLoggerBase`.

snippet:ConfigureLogging

This code uses self hosting console scenario for example purposes. Typically the `EventListener` would be instantiated at service startup and disposed of in service shutdown. Depending on the hosting approach it may not be possible to have a `using` wrap the instances. See also [Hosting](/nservicebus/hosting/) and [Windows Service Hosting](/nservicebus/hosting/windows-service.md).