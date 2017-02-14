The `NHibernateStorageContext` can be used directly to access NHibernate `ISession`.


### Using in a Handler

snippet:NHibernateAccessingDataViaContextHandler


### Using in a Saga

snippet:NHibernateAccessingDataViaContextSaga

include: saga-business-data-access


`ISession` instances can also be injected directly into the handlers or sagas but it requires explicit configuration.

snippet:NHibernateAccessingDataDirectlyConfig

snippet:NHibernateAccessingDataDirectly

The first part instructs that the `ISession` instance should be injected into the handlers and the second part uses constructor injection to access the `ISession` object.