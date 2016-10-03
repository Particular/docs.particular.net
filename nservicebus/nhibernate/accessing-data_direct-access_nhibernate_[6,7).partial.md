Note that in Version 5 and below, `ISession` could be injected directly into the handlers. This behavior has changed in NServiceBus Version 6 as internal components of NServiceBus are no longer accessible from the [container](/nservicebus/containers/). This approach can still be used on Version 5 and below.

snippet:NHibernateAccessingDataDirectlyConfig

snippet:NHibernateAccessingDataDirectly

The first part instructed NServiceBus to inject the `ISession` instance into the handlers and the second part uses constructor injection to access the `ISession` object.