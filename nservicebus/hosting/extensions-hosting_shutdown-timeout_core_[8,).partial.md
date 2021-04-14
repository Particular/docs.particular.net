### Shutdown timeout

The Microsoft generic host has a [configurable shutdown timeout that is defaulted to five seconds](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host#shutdowntimeout). NServiceBus will by default wait until all in-flight messages complete and to make sure that the shutdown timeout is honored all message handlers needs to [observe the cancellation token that is available on the message handler context](LINK TO NEW DOCO PAGE).
