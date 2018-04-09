The default action should be overridden in the following scenarios:

 * When using [NServiceBus Host](/nservicebus/hosting/nservicebus-host), in case some custom operations should be performed before the endpoint process is exited.
 * When self hosting and the default behavior isn't desired

NOTE: If the endpoint is stopped without exiting the process, then any `Send` or `Publish` operation will result in an [KeyNotFoundException](https://msdn.microsoft.com/en-us/library/system.collections.generic.keynotfoundexception) being thrown.
