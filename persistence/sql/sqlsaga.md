---
title: SqlSaga base class
summary: An alternate SqlSaga base class for use with SQL persistence
component: SqlPersistence
related:
 - persistence/sql/saga
reviewed: 2025-04-10
---

`SqlSaga<T>` is an alternate saga base class for use with SQL persistence that offers a less verbose mapping API than `NServiceBus.Saga<T>`. It's generally advisable to use `NServiceBus.Saga<T>` by default for most new projects, and to switch to `SqlSaga<T>` when advantageous to cut down on the need for repetitive `.ToSaga(...)` expressions in sagas that handle several message types.

partial: required-in-some-versions

partial: main-content