
#### .NET Core 2.0 unsupported

The default transaction mode is currently not suppoed with .NET Core 2.0 and will result in the following exception. Choose one of the other transaction modes to resolve this.
```
NServiceBus.Transport.SQLServer.MessagePump|Sql receive operation failed
System.NotSupportedException: Enlisting in Ambient transactions is not supported.
```

WARNING: The Transaction scope mode is NOT available in .NET Core 2.0 because the implementation of `SqlConnection` does not support enlisting in an ambient transaction.

