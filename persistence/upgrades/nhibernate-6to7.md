---
title: NHibernate Persistence Upgrade Version 6 to 7
summary: Instructions on how to upgrade NHibernate Persistence Version 6 to 7.
reviewed: 2016-03-16
component: NHibernate
related:
 - nservicebus/upgrades/5to6
redirects:
 - nservicebus/upgrades/nhibernate-6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Accessing the ISession

The way the NHibernate's `ISession` object is accessed has changed. This object is no longer accessible through the IoC Container or `NHibernateStorageContext`. When Property or Constructor injection is used to get an instance of `ISession` directly or through `NHibernateStorageContext`, the code needs to be refactored after the upgrade to this version.

snippet: NHibernateAccessingSessionUpgrade6To7

As shown in the above snippet, the only way to access the `ISession` object is through the `Session()` extension method on `IMessageHandlerContext.SynchronizedStorageSession`.

The reasoning behind removing the registration from the IoC Container was not exposing internal components of NServiceBus as much as possible and having less behavioral changes in future versions. As such, the extension method called `RegisterManagedSessionInTheContainer()` which used to enable this behavior is made obsolete and can be removed.


## Customizing the ISession Creation

It is no longer possible to customize `ISession` creation process. As such, the extension method call `UseCustomSessionCreationMethod()` is no longer needed and should be removed.

Since most of the methods available on the `ISessionFactory` are related to caching and with this [existing NHibernate bug](https://nhibernate.jira.com/browse/NH-3023) (see also [accessing data](/persistence/nhibernate/accessing-data.md)), the benefit of having customizable `ISession` creation is debatable. To customize the created `ISession` object, for example to apply a filter on the session level, the code needs to be moved to the handler and applied to the `ISession` object on the Context.


## Unique attribute no longer needed

NServiceBus will automatically make the correlated saga property unique without the need for an explicit `[Unique]` attribute to be used. This attribute can be safely removed from saga data types.

Refer to the [NServiceBus upgrade guide for migrating from Version 5 to 6](/nservicebus/upgrades/5to6/handlers-and-sagas.md) to learn more about changes regarding sagas.