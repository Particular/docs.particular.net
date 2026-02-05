> [!WARNING]
> Other than interacting with its own internal state, a saga should not access a database, call out to web services, or access other resources. See [Accessing databases and other resources from a saga](/nservicebus/sagas/#avoid-external-resource-access).

If the situation is special enough to warrant going against this recommendation, the following documentation will describe how to do so.
