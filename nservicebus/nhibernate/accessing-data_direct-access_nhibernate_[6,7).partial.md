snippet:NHibernateAccessingDataViaContext

As shown above, `NHibernateStorageContext` can be used directly to access NHibernate `ISession`.

`ISession` instances can also be injected directly into the handlers but it requires explicit configuration.

snippet:NHibernateAccessingDataDirectlyConfig

snippet:NHibernateAccessingDataDirectly

The first part instructed NServiceBus to inject the `ISession` instance into the handlers and the second part uses constructor injection to access the `ISession` object.