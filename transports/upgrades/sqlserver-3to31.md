---
title: SQL Server Transport Upgrade Version 3 to 3.1
summary: Instructions on how to migrate the delayed messages from persistence-based mechanism (Timeout Manager) to native transport handling.
reviewed: 2021-03-02
component: SqlTransport
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---

SQL Server transport 3.1 introduces native handling of delayed messages. It does so via a dedicated table which holds messages that have been sent but are not yet due. The snippet below shows T-SQL script that creates delayed messages table:

```sql
IF EXISTS (
    SELECT *
    FROM {1}.sys.objects
    WHERE object_id = OBJECT_ID(N'{0}')
        AND type in (N'U'))
RETURN

EXEC sp_getapplock @Resource = '{0}_lock', @LockMode = 'Exclusive'

IF EXISTS (
    SELECT *
    FROM {1}.sys.objects
    WHERE object_id = OBJECT_ID(N'{0}')
        AND type in (N'U'))
BEGIN
    EXEC sp_releaseapplock @Resource = '{0}_lock'
    RETURN
END

CREATE TABLE {0} (
    Headers nvarchar(max) NOT NULL,
    Body varbinary(max),
    Due datetime NOT NULL,
    RowVersion bigint IDENTITY(1,1) NOT NULL
);

CREATE NONCLUSTERED INDEX [Index_Due] ON {0}
(
    [Due]
)

EXEC sp_releaseapplock @Resource = '{0}_lock'
```

In order to drain all delayed messages sent before upgrading to version 3.1, [Timeout Manager](/nservicebus/messaging/timeout-manager.md) is enabled by default in version 3.1 of the transport.

As some delayed messages can have the due times months or even years in the future it might be advisable to move the messages manually from the old Timeout Manager storage to the native delayed tables. See the [SQL Server Native Delayed Delivery](/transports/sql/native-delayed-delivery.md) article for more details.


### SQL Server

If SQL Server was used as a backing store for the Timeout Manager, either via [NHibernate persistence](/persistence/nhibernate/) or [SQL persistence](/persistence/sql), refer to [the timeouts migration tool documentation](/nservicebus/tools/migrate-to-native-delivery.md) for details.


### Other databases

If another database was used, use DB-specific tools to extract the `Headers`, `State`, and `Destination` values from the timeout records and export the result to a file.

NOTE: Some persisters, e.g. NHibernate, store all the delayed messages for all the endpoints in a single table. Exporting just the ones for the endpoint that is migrated requires filtering on the `Endpoint` property of the timeout record.

Once exported, use the following script to insert the data into SQL Server transport's table.

```sql
DECLARE @NOCOUNT VARCHAR(3) = 'OFF';
IF ( (512 & @@OPTIONS) = 512 ) SET @NOCOUNT = 'ON'
SET NOCOUNT ON;

INSERT INTO {0} (
    Headers,
    Body,
    Due)
VALUES (
    @Headers,
    @Body,
    @Due);

IF(@NOCOUNT = 'ON') SET NOCOUNT ON;
IF(@NOCOUNT = 'OFF') SET NOCOUNT OFF;
```

NOTE: By default, the table used to store delayed messages has the `Delayed` suffix so for an endpoint called `MyEndpoint` the delayed messages are stored in a table called `MyEndpoint.Delayed`.
