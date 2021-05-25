### Shutdown timeout

The .NET Generic Host has a [configurable shutdown timeout that defaults to five seconds](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host#shutdowntimeout). During this shutdown period, NServiceBus waits for all in-flight messages to complete. To ensure the shutdown timeout is honored, all message handlers must [observe the cancellation token that is available on the message handler context](/nservicebus/hosting/cooperative-cancellation.md).
