---
title: SQL Server Transport Upgrade Version 3 to 3.1
summary: Instructions on how to migrate the delayed messages from persistence-based mechanism (Timeout Manager) to native transport handling.
reviewed: 2017-07-06
component: SqlTransport
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---

SQL Server transport 3.1 introduces a native handling of delayed messages. It does so via a special table that holds the messages that are sent but not yet due. The structure of this table is shown below:

snippet: 3to31-createdelayedmessagestoretextsql

SQL Server transport 3.1 by default runs the [Timeout Manager](/nservicebus/messaging/timeout-manager.md) using the selected persistence option to drain all the remaining delayed messages sent before upgrading to version 3.1. However, even the new delayed messages are processed using the native mechanism only even when they are sent by the endpoint using older version of the transport.

Because some delayed messages can have the due times months or even years in future it might be advisable to migrate them in order to be able to disable the Timeout Manager entirely. See the [SQL Server Native Delayed Delivery](/transports/sql/native-delayed-delivery.md) article for more details.


### SQL Server

If SQL Server was used as a backing store for the Timeout Manager, either via [NHibernate persistence](/persistence/nhibernate/) or [SQL persistence](/persistence/sql), refer to [this sample](/samples/sqltransport/native-timeout-migration/) for details on how to migrate. 


### Other databases

If another database was used, use DB-specific tools to extract the `Headers`, `State`, and `Destination` values from the timeout records and export the result to a file.

NOTE: Some persistences, e.g. NHibernate, store all the delayed messages for all the endpoints in a single table. Exporting just the ones for the endpoint that is migrated requires filtering on the `Endpoint` property of the timeout record.

Once exported, use the following script to insert the data into SQL Server transport's table.

snippet: 3to31-storedelayedmessagetextsql

NOTE: By default the table used to store delayed messages has the `Delayed` suffix so for an endpoint called `MyEndpoint` the delayed messages are stored in a table called `MyEndpoint.Delayed`.
