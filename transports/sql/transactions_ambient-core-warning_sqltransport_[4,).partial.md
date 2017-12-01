
#### Transaction scope mode is not supported in .NET Core 2.0

The default transaction mode, i.e. transaction scope, is currently not supported in .NET Core 2.0. Any attempt to use it will result in the following exception:

```
NServiceBus.Transport.SQLServer.MessagePump|Sql receive operation failed
System.NotSupportedException: Enlisting in Ambient transactions is not supported.
```

To resolve this, choose one of the other transaction modes. Support for `TransactionScope` is expected to be added in future releases of .NET Core.
