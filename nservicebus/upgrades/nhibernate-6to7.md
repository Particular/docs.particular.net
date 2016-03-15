---
title: NServiceBus NHibernate upgrade from Version 6 to Version 7
summary: Instructions on how to upgrade from NServiceBus.NHibernate Versions 6 to 7
tags:
 - upgrade
 - migration
related:
- nservicebus/nhibernate
---


## NServiceBus NHibernate

### Accessing the ISession

The way the NHibernate's `ISession` object is accessed has changed. This object is no longer accessible through the IoC Container or `NHibernateStorageContext`. When Property or Constructor injection is used to get an instance of `ISession` directly or through `NHibernateStorageContext`, the code needs to be refactored after you upgrade to this version.

snippet:NHibernateAccessingDataViaContext

As shown in the above snippet, the only way to access the `ISession` object is through the `Session()` extension method on `IMessageHandlerContext.SynchronizedStorageSession`. 

The reasoning behind removing the registration from the IoC Container was not exposing internal components of NServiceBus as much as possible and having less behavioral changes in future versions. As such, the extension method called `RegisterManagedSessionInTheContainer()` which used to enable this behavior is made obsolete and can be removed from your code.

### Customizing the ISession Creation

It is no longer possible to customize `ISession` creation process. As such, the extension method call `UseCustomSessionCreationMethod()` is no longer needed and should be removed. 

Since most of the methods available on the `ISessionFactory` are related to caching and with this [existing NHibernate bug][1] (also listed [here][2] as a known limitation), the benefit of having customizable `ISession` creation is debatable. To customize the created `ISession` object, for example to apply a filter on the session level, the code needs to be moved to the handler and appllied to the `ISession` object on the Context.

[1]: https://nhibernate.jira.com/browse/NH-3023
[2]: http://docs.particular.net/nservicebus/nhibernate/accessing-data