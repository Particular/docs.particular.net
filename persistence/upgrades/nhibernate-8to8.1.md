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


NHibernate 5, which is used by the NHibernate persistence starting from Version 8.1, defaults to using `DateTime2(7)` T-SQL data type when accessing the database. Because the `DateTime2(7)` type is more precise than the `DateTime` type, this has been deemed a safe change as it does not cause any data loss. It does, however, cause issues when using `DateTime` columns for optimistic concurrency enforcement.


## Optimistic concurrency control

Optimistic concurrency checking is used when the persistence needs to read a value from a database, calculate a derived value and store that derived value back in the database. To make sure such it correctly works the persister needs to ensure the value the calculation was based on has not been modified in the meantime. An example would be incrementing a counter.

Processes A and B read value `5` as the current value of the counter and both processes increment the value in memory. Then both processes update the database. Without the optimistic concurrency check, the outcome is `6` which is incorrect because it is expected to increment the value twice.

With optimistic concurrency check the processes would append `where counter = 5` clause to the update statement to ensure that the value being updated has not been modified in the meantime. One of the processes would succeed and the other would fail (no rows updated) and would have to re-try, this time reading value `6` as the current counter.


## Accessing DateTime columns using DateTime2 type

By default, NHibernate persistence uses all Saga fields for optimistic concurrency control. The generated update statement contains a `where` clause with all previously read Saga property values.

Reading `DateTime` columns into a .NET `DateTime` type is a lossy conversion because the database type's precision is 1/300 of a second, a value that cannot be represented in the .NET type. This means that when the least significant digit of the millisecond value is not 0, the fraction part is lost. It can be observed how SQL Server rounds up the value to the 1/300 increments by executing the following code

```
DECLARE @datetime datetime = '2016-10-23 12:45:37.335';
SELECT @datetime AS '@datetime'
```

The actual number of milliseconds stored is 336 and two-thirds but when the value is read in .NET, it gets rounded up to 337.

Later, when building the update command, NHibernate passes that converted value as the `DateTime2(7)` type. This means that the database understands that the value is as precise as this new type permits: `2016-10-23 12:45:37.3370000`.

Because the value in the table is of `DateTime` type the SQL Server engine up-converts it to match the more precise `DateTime2(7)` in order to compare equality. Based on the [new conversions rules in SQL Server 2016](https://support.microsoft.com/en-us/help/4010261/sql-server-and-azure-sql-database-improvements-in-handling-data-types) the conversion yields `2016-10-23 12:45:37.3366666` which is not equal to `2016-10-23 12:45:37.3370000`. As a result, the update statement does not modify any rows which triggers NHibernate to raise `NHibernate.StaleStateException` to indicate that the value has been changed since it has been read.


## Upgrade paths

Because, as mentioned, NHibernate 5 defaults to `DateTime2(7)` type, all new saga tables will use that new type for storing .NET `DateTime` values. These new sagas will work correctly with Version 8.1 of NHibernate persistence.

All the existing sagas that use `DateTime` type in their tables stop working with Version 8.1 of NHibernate persistence due to the optimistic concurrency problem manifesting in `NHibernate.StaleStateException` being thrown. There are three upgrade paths to make these existing sagas work.


### Alter schema

One way to solve the problem is to alter the schema of existing saga tables to use the new `DateTime2(7)` data type. Such alteration can be done in-place without any data/precision loss.


### Use explicit version field

NHibernate persistence has an option to use [explicit version column](/persistence/nhibernate/saga-concurrency.md#explicit-version) for optimistic concurrency checks. This is usually a better option because the resulting update command is much simpler. Enabling the explicit versioning requires changing the sagas code and altering the saga table schema (adding a column). 


### Force backward compatibility

If altering the schema is not an option, NHibernate offers a configuration switch to fall back to using the classic `DateTime` data type

snippet: NHibernateLegacyDateTimeUpgrade7To8

The downside of this approach is that this is a global setting and affects all tables mapped using that NHibernate configuration object.


### Switch to lower database compatibility mode

Because the comparison in the update statement fails due to improved conversion rules of SQL Server 2016 another way to solve the problem is to switch the database compatibility mode to SQL Server 2014 (120).

```
USE [database_name]
GO
ALTER DATABASE CURRENT SET COMPATIBILITY_LEVEL=120
GO
```
