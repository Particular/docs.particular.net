
The `NHibernateStorageContext` can be used directly to access NHibernate `ISession`.

NOTE: If different connections strings were used for particular persistence features, such as sagas and timeouts, then `context.SynchronizedStorageSession.Session()` will expose connection string for sagas.


### Using in a handler

snippet: NHibernateAccessingDataViaContextHandler


### Using in a saga

include: saga-business-data-access

snippet: NHibernateAccessingDataViaContextSaga
