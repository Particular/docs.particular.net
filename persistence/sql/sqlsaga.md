---
title: SqlSaga base class
summary: An alternate SqlSaga base class for use with SQL persistence
component: SqlPersistence
related:
 - persistence/sql/saga
reviewed: 2025-12-05
---

> [!WARNING]
> Starting in version 8.3.0, the `SqlSaga<T>` class is deprecated. With the [simplified message correlation syntax](/nservicebus/sagas/message-correlation.md) that does not repeat the `.ToSaga(…)` mapping, the alternate saga base class is no longer necessary.
>
> When using a standard `Saga<T>` base class, the correlation id is determined by analysis of the `ConfigureHowToFindSaga` method.
>
> A [transitional correlation id](/persistence/sql/saga.md#correlation-ids-transitional-correlation-id) (a feature exlusive to SQL Persistence) can still be applied using the `[SqlSaga]` attribute applied to a saga class.
>
> The generated [table name](/persistence/sql/saga.md#table-structure-table-name) can also be set using the `[SqlSaga]` attribute.

partial: required-in-some-versions

partial: main-content