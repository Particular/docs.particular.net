---
title: SQL Persistence Upgrade Version 8 to 9
summary: Migration instructions on how to upgrade to SQL Persistence version 9
reviewed: 2025-12-09
component: SqlPersistence
related:
- persistence/sql
- nservicebus/upgrades/9to10
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
 - 10
---

## Script generation via source generators

In version 9, the script generation that creates SQL scripts for sagas at compile time now uses Roslyn as the engine for analyzing saga classes.

Previously, SQL script generation used the Mono.Cecil package to inspect the just-compiled assembly at the Intermediate Language (IL) level, stepping through individual compiler instructions (similar to assembly code) in order to infer a saga class's correlation id. Although relatively stable, the code to do it was quite complex as it had to account for all the things a person might potentially do in the instructions of a `ConfigureHowToFindSaga` method that have nothing to do with the correlation id. There was also always the risk that a new version of the .NET SDK would change something about how familiar saga mapping code was represented in IL instructions that would break the instruction analyzer.

In this process, the MSBuild task that creates the SQL scripts had to load the just-compiled assembly and inspect it at a fairly deep level, enumerating through types and inspecting the implementation of specific methods.

In version 9, the analysis is done using a Roslyn incremental source generator, which makes it resident inside the compiler at compile time. At this level of abstraction, the analysis takes place at the level of the saga code's syntax tree and semantic model, which creates a much more reliable way to confidently determine the correlation property by finding the call to `mapper.MapSaga(…)` and inspecting the expression inside.

The source generator inspects the sagas during compilation and writes out a code file with one assembly-level attribute for each saga found, each containing the metadata for one saga type. The MSBuild task is then able to perform a shallow analysis of the just-compiled assembly to extract only these metadata attributes and then write the SQL scripts.

### Visual Basic sagas not supported

Currently, sagas authored with Visual Basic .NET are not supported in version 9, as there is currently insufficient evidence of the use of VB.NET sagas. Support for VB.NET would require a separate VB.NET source generator that was not created given the lack of evidence. If affected by this change, [contact support](https://particular.net/support).

## `SqlSaga` class deprecated

The `SqlSaga<T>` base class, an alternate base class for defining a [saga](/nservicebus/sagas/), is deprecated with a warning starting in NServiceBus.Persistence.Sql version 8.3.0 and generates an error in version 9.0.0.

The `SqlSaga` class existed primarily to make it easier to identify a saga's [correlation id](/nservicebus/sagas/message-correlation.md) so that it could be used to [generate SQL scripts](/persistence/sql/#script-creation) for saga classes, and to provide a simplified (but experimental) mapping syntax that was ultimately made unnecessary by improvements to the main NServiceBus saga mapping syntax. `SqlSaga` also did not benefit from the [Roslyn analyzers and code fixes for sagas](/nservicebus/sagas/analyzers.md) that help ensure sagas don't fall into various antipatterns.

### Converting SqlSaga

Each `SqlSaga<T>` will define a `ConfigureMapping` method and override the `CorrelationPropertyName`, such as:

```csharp
// SQL Persistence SqlSaga<T>
protected override void ConfigureMapping(IMessagePropertyMapper mapper)
{
    mapper.ConfigureMapping<StartOrder>(message => message.OrderId);
    mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId);
}

protected override string CorrelationPropertyName => nameof(MySagaData.OrderId);
```

Instead, use the `NServiceBus.Saga<TSagaData>` base class, and express the same mappings together by overriding the `ConfigureHowToFindSaga` method. Note how the calls to `ToMessage<TMessage>(…)` can be chained:

```csharp
// NServiceBus Saga<TSagaData>
protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
{
    mapper.MapSaga(sagaData => sagaData.OrderId)
        .ToMessage<StartOrder>(message => message.OrderId)
        .ToMessage<CompleteOrder>(message => message.OrderId);
}
```

A `SqlSaga` may also override the `TransitionalCorrelationPropertyName` property to generate a SQL table that can facilitate transitioning from one correlation property to a new strategy. This feature can still be used via the [`[SqlSaga]` attribute](/persistence/sql/saga.md#correlation-ids-specifying-correlation-id-using-an-attribute):

```csharp
[SqlSaga(
    correlationProperty: nameof(MySagaData.OrderId),
    transitionalCorrelationProperty: nameof(MySagaData.LegacyOrderId)
)]
public class MySaga : Saga<MySagaData>
{
    // Saga implementation
}
```

Additionally, a `SqlSaga` may override the `TableSuffix` property to control the name of the table that is created by the SQL scripts. This can also be accomplished using the [`[SqlSaga]` attribute](/persistence/sql/saga.md#table-structure-table-name):

```csharp
[SqlSaga(tableSuffix: "CustomOrderSagaTable")]
public class MySaga : Saga<MySagaData>
{
    // Saga implementation
}
```