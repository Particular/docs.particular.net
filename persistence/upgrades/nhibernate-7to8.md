---
title: NHibernate Persistence Upgrade Version 8.0 to 8.1
summary: Instructions on how to upgrade NHibernate Persistence Version 8.0 to 8.1.
reviewed: 2019-04-05
component: NHibernate
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 7
---


NHibernate 5, which is used by the NHibernate persistence starting from Version 8.1, defaults to using `DateTime2(7)` T-SQL data type when accessing the database. Because the `DateTime2(7)` type is more precise than the `DateTime` type this has been deemed a safe change as it does not cause any data loss. It does, however, cause issues when using `DateTime` columns for optimistic concurrency enforcement.

## Optimistic concurrency control

Optimistic concurrency checking is used when a code needs to read a value from a database, calculate a derived value and store that derived value back in the database. To make sure such code works correctly it needs to ensure that the value the calculation was based has not been modified in the meantime. A good example would be incrementing a counter.

Processes A and B read value `5` as the current value of the counter and both processes increment the value in memory. Then both processes update the database. Without the optimistic concurrency check the outcome is `6` which is incorrect because we expected to increment the value twice.

With optimistic concurrency check the processes would append `where counter = 5` clause to the update statement to ensure that the value being updated has not been modified. One of the processes would succeed and the other would fail (no rows updated) and would have to re-try, this time reading value `6` as the current counter.

## Accessing DateTime columns using DateTime2 type

By default, NHibernate persistence uses all Saga fields for optimistic concurrency control so the generated update statement contains a `where` clause with all previously read Saga property values.

When reading `DateTime` column values using `DateTime2(7)` type the value is read without any precision loss and stored in the .NET `DateTime` type. Later, when building the update command NHibernate passes the `DateTime2(7)` type as the command parameter type which causes ADO.NET to send a `DateTime2(7)` value to the database rather than `DateTime` value. Because these are different types, the comparison fails resulting in `ObjectStaleException` being thrown by NHibernate.

## Upgrade paths

Because, as mentioned, NHibernate 5 defaults to `DateTime2(7)` type, all new saga tables will used that new type for storing .NET `DateTime` values. These new sagas will work correctly with Version 8.1 of NHibernate persistence.

All the existing sagas that use `DateTime` type in their tables stop working with Version 8.1 of NHibernate persistence due to the optimistic concurrency problem manifesting in `ObjectStaleException` being thrown. There are three upgrade paths to make these existing sagas work.

### Alter schema

One way to solve the problem is to alter the schema of existing saga tables to use the new `DateTime2(7)` data type. Such alteration can be easily done in-place without any data/precision loss.

### Use explicit version field

NHibernate persistence has an option to use [explicit version column](/persistence/nhibernate/saga-concurrency.md#explicit-version) for optimistic concurrency checks. This is usually a better option because the resulting update command is much simpler. Enabling the explicit versioning requires changing the sagas code and altering the saga table schema (adding a column). 

### Force backward compatibility

If altering the schema is not an option, NHibernate offers a configuration switch to fall back to using the classic `DateTime` data type

snippet: NHibernateLegacyDateTimeUpgrade7To8

The downside of this approach is that this is a global setting and affects all tables mapped using that NHibernate configuration object.