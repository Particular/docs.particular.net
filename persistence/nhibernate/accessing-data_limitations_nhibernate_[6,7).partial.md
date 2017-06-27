## Known limitations

When the [transport transaction](/transports/transactions.md) mode is set to TransactionScope, NServiceBus opens a session by passing an existing instance of a database connection. Therefore, it is not possible to use NHibernate's second-level cache. The reason why sessions are opened this way in NServiceBus is because of an [unresolved bug in NHibernate](https://nhibernate.jira.com/browse/NH-3023).

In other transport transaction modes the session is created using a parameterless `OpenSession()` and an explicit NHibernate transaction is used which makes it possible to take advantage of the second-level cache.