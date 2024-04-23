### Using the built-in clean-up method

Specify a value for the `TimeToBeReceived` property. For more details on how to specify this, see [Discarding Old Messages](/nservicebus/messaging/discard-old-messages.md).

> [!WARNING]
> The built-in method uses continuous blob scanning which can add to the cost of the storage operations. It is **not** recommended for multiple endpoints that are scaled out. If this method is not used, be sure to disable the built-in cleanup by setting the `CleanupInterval` to `0`. In versions 3 and above built-in cleanup is disabled by default.
