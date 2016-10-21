## Known limitations

When the [transport transactions](/nservicebus/transports/transactions.md) are set to `TransactionScope` mode, due to the way NServiceBus opens sessions (by passing an existing instance of a database connection), it is currently not possible to use NHibernate's second-level cache. Such behavior of NServiceBus is caused by [still-unresolved bug in NHibernate](https://nhibernate.jira.com/browse/NH-3023).

In other transport transaction modes the session is created using a parameterless `OpenSession()` and an explicit NHibernate transaction is used which makes it possible to take advantage of the second-level cache.