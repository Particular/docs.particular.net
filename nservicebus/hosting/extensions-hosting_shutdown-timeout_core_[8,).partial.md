### Shutdown timeout

The generics host has a [configurable shutdown timeout that is defaulted to five seconds](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host#shutdowntimeout). NServiceBus will by default wait until all in-flight message handlers complete so to make sure that shutdown timeout is honored all message handlers needs to [observe the cancellation token that is available on the message handler context](LINK TO NEW DOCO PAGE).
