
#### .NET Core 2.0 unsupported

The default transaction mode, i.e. transaction scope, is currently not supported in .NET Core 2.0. The attempt to use it will result in the following exception:

```
NServiceBus.Transport.SQLServer.MessagePump|Sql receive operation failed
System.NotSupportedException: Enlisting in Ambient transactions is not supported.
```
Choose one of the other transaction modes to resolve this.

WARNING: The Transaction scope mode is NOT available in .NET Core 2.0 because the implementation of `SqlConnection` does not support enlisting in an ambient transaction.
