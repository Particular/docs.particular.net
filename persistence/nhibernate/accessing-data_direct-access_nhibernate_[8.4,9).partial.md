
NServiceBus-managed `ISession` instance can be accessed using the handler context through `context.SynchronizedStorageSession.Session()` or by using the dependency injection (DI).

### Using in a handler

snippet: NHibernateAccessingDataViaContextHandler

snippet: NHibernateAccessingDataViaDI

NOTE: A helper type `INHibernateStorageSession` is used in the example above because by default `ISession` is not registered in the DI container. Users who wish to register `ISession` need to add the following line to the endpoint configuration:

snippet: AccessingDataConfigureISessionDI


### Using in a saga

include: saga-business-data-access

snippet: NHibernateAccessingDataViaContextSaga
