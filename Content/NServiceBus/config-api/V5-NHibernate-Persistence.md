---
title: Configuration API NHibernate Persistence in V5
summary: Configuration API NHibernate Persistence in V5
tags:
- NServiceBus
- BusConfiguration
- V5
- Persistence
---

To configure the persitence engine to use NHibernate as the persitence ORM call the `UsePersistence<NHibernatePersistence()`, or the `UsePersistence( typeof( NHibernatePersistence ) )`, method. The `NServiceBus.NHibernate` persistence package adds some behaviors to the default `PersitenceExtensions<TPersistence>` instance:

* `DisableGatewayDeduplicationSchemaUpdate`: Disables the Gateway Deduplication schema updates;
* `DisableSubscriptionStorageSchemaUpdate`: Disables the Subscription Storage schema updates;
* `DisableTimeoutStorageSchemaUpdate`: Disables the Timeout Storage schema updates;
* `SagaTableNamingConvention`: Allow to define the conventions used for Sagas table namings;
* `UseGatewayDeduplicationConfiguration`: Defines the configuration to use for Gateway Deduplication;
* `UseSubscriptionStorageConfiguration`: Defines the configuration to use for the Subscription Storage;
* `UseTimeoutStorageConfiguration`: Defines the configuration to use for the Timeout Storage;
* `EnableCachingForSubscriptionStorage`: Enables the usage of caching for Subscriptions;