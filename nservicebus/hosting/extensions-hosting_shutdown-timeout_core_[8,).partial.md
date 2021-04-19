### Shutdown timeout

The Microsoft generic host has a [configurable shutdown timeout that is defaulted to five seconds](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host#shutdowntimeout). During this shutdown period, NServiceBus will attempt to wait until all in-flight messages complete. To make sure that the shutdown timeout is honored, all message handlers need to [observe the cancellation token that is available on the message handler context](/nservicebus/hosting/cooperative-cancellation.md).
