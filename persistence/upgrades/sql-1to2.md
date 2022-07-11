---
title: SQL Persistence Upgrade Version 1 to 2
summary: Instructions on how to upgrade to SQL Persistence version 2
reviewed: 2020-11-02
component: SqlPersistence
isUpgradeGuide: true
redirects:
 - nservicebus/upgrades/sqlpersistence-1to2
 - persistence/upgrades/sqlpersistence-1to2
upgradeGuideCoreVersions:
 - 6
---


## Subscription Caching configuration now required

[Subscription Caching](/persistence/sql/subscriptions.md) is now a required configuration option. 

Either configure a time to cache for:

```csharp
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));
```

Or explicitly disable the subscription caching.

```csharp
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var subscriptions = persistence.SubscriptionSettings();
subscriptions.DisableCache();
```


## Inheriting from SqlSaga now required

In Version 1, inheriting from `NServiceBus.Saga<T>` was partially supported. However, this has two competing approaches that deliver the same features, causing significant confusion. In Version 2, `NServiceBus.Saga<T>` is no longer supported, and either a build error, or a runtime exception for some edge cases will occur.


## Deep nested Saga hierarchies are no longer supported

In Version 1, having deep class hierarchies inheriting from `SqlSaga<T>` was supported. This scenario is no longer supported, and all sagas **must** inherit directly from `SqlSaga<T>`. This decision was made to bring the SQL persistence inline with the future design of NServiceBus.


## Correlation Property

The API for defining a Correlation Property has been moved from an attribute to a property at the saga class level.

```csharp
// For Sql Persistence version 2.x
public class MySaga :
    SqlSaga<MySaga.SagaData>,
    IAmStartedByMessages<MyMessage>
{
    protected override string CorrelationPropertyName => nameof(SagaData.TheId);

// For Sql Persistence version 1.x
[SqlSaga(
    correlationProperty: nameof(SagaData.TheId)
)]
public class MySaga :
    SqlSaga<MySaga.SagaData>,
    IAmStartedByMessages<MyMessage>
{
```


## Message Mapping

The API to define message mapping has been changed to bring it in line with the future design of NServiceBus:

 * `MapMessage` renamed to `ConfigureMapping`.
 * `MessagePropertyMapper<T>` renamed to `IMessagePropertyMapper`.

```csharp
// For Sql Persistence version 2.x
protected override void ConfigureMapping(IMessagePropertyMapper mapper)
{
    mapper.ConfigureMapping<MyMessage>(_ => _.TheId);
}

// For Sql Persistence version 1.x
protected override void ConfigureMapping(MessagePropertyMapper<SagaData> mapper)
{
    mapper.MapMessage<MyMessage>(_ => _.TheId);
}
```


## SqlSaga.ConfigureMapping made abstract

To simplify implementing a saga using `SqlSaga<T>` class, the method `SqlSaga<T>.ConfigureMapping` has been turned into an abstract method which now needs to be implemented even if no message mapping is required.


## SqlPersistenceSettingsAttribute move to use properties

Attributes have been moved to use properties instead of optional parameters in the constructor.

```csharp
// For Sql Persistence version 2.x
[assembly: SqlPersistenceSettings(
    MsSqlServerScripts = true,
    MySqlScripts = true)]

// For Sql Persistence version 1.x
[assembly: SqlPersistenceSettings(
    msSqlServerScripts: true,
    mySqlScripts: true)]
```


## SqlSagaAttribute made obsolete

The `[SqlSagaAttribute]` has been made obsolete and replaced by property overrides on the `SqlSaga<T>` class.

```csharp
// For Sql Persistence version 2.x
public class MySaga :
    SqlSaga<MySaga.SagaData>
{
    protected override string CorrelationPropertyName => nameof(SagaData.CorrelationProperty);
    protected override string TransitionalCorrelationPropertyName => nameof(SagaData.TransitionalCorrelationProperty);
    protected override string TableSuffix => "TheCustomTableName";

// For Sql Persistence version 1.x
[SqlSaga(
    tableSuffix: "TheCustomTableName",
    transitionalCorrelationProperty: "OtherPropertyName"
)]
```


## Explicit schema API

An explicit schema API has been added.

```csharp
// For Sql Persistence version 2.x
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.Schema("MySchema");

// For Sql Persistence version 1.x
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.TablePrefix("MySchema.");
```

If characters that required quoting were previously used in the table prefix, they can be removed and the following can be used instead:

```csharp
// For Sql Persistence version 2.x
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.Schema("My Schema");

// For Sql Persistence version 1.x
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.TablePrefix("[My Schema].");
```

WARNING: An exception will be thrown if any of ], [ or &grave; are detected in the `tablePrefix` or the schema.


## Missing Indexes

Some missing indexes have been added. These indexes will be added the next time the [installers](/persistence/sql/install.md) are executed. No explicit SQL migration is required.


### TimeoutData

 * Add missing non-unique index on `Time`, which is used to query expired timeouts.
 * Add missing non-unique index on `SagaId`, which is used to clean timeouts of the completed sagas.


### OutboxData

* Add missing index on `Dispatched (bool)`
* Add missing index on `DispatchedAt (datetime)`
