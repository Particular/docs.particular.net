`ISession` instances can also be injected directly into the handlers.

NOTE: If different connections strings were used for particular persistence features such as sagas, timeouts, etc. then `context.SynchronizedStorageSession.Session()` will expose connection string for sagas.

snippet: NHibernateAccessingDataDirectly

The handler uses constructor injection to access the `ISession` object.
