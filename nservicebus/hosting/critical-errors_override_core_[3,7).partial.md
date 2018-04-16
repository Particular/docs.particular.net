Warning: The default action should always be overridden when self-hosting NServiceBus. By default, when a critical error occurs, the endpoint will stop without exiting the process.

NOTE: If the endpoint is stopped without exiting the process, then any `Send` operation will result in an [ObjectDisposedException](https://msdn.microsoft.com/en-us/library/system.objectdisposedexception.aspx) being thrown.
