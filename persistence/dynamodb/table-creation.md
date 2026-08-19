---
title: Table creation (DynamoDB)
summary: How to create the table used by DynamoDB persistence instead of relying on installers
component: DynamoDB
reviewed: 2026-08-19
related:
- persistence/dynamodb
- nservicebus/operations/installers
- nservicebus/aws/local-development
---

When [installers](/nservicebus/operations/installers.md) are enabled, an endpoint creates the table it needs at startup. This is convenient during development, but it requires the endpoint to run with permission to create and configure tables, which it never needs once the table exists.

In production, the deployment process usually creates the table before the endpoint is deployed. This article describes the table that DynamoDB persistence expects, and shows how to create it with the AWS CLI. CloudFormation, Terraform, or any other deployment mechanism can create the same table.

## Installers

An endpoint only creates tables at startup when installers are enabled:

```csharp
endpointConfiguration.EnableInstallers();
```

When the deployment process creates the table, omit `EnableInstallers`. If installers are needed for other reasons, disable only the table creation:

snippet: DynamoDBDisableTableCreation

## Permissions

Creating the table becomes the responsibility of the deployment process, so use separate deployment and application identities where possible. The deployment identity needs permission to create and configure the table. The application identity needs only runtime data access, not the table management permissions that installers require.

The minimum permission set for each is documented in [permissions](/persistence/dynamodb/#permissions).

## Table schema

DynamoDB persistence stores saga and outbox data in a table with a composite key:

| Element | Default | Requirement |
| --- | --- | --- |
| Table name | `NServiceBus.Storage` | Must match `TableConfiguration.TableName` if customized |
| Partition key | `PK` | String (`S`), `HASH` key |
| Sort key | `SK` | String (`S`), `RANGE` key |
| Time-to-live attribute | `ExpiresAt` | Time-to-live enabled on this attribute |

The names are configurable via `UseSharedTable`, or separately per feature via the [saga](/persistence/dynamodb/sagas.md) and [outbox](/persistence/dynamodb/outbox.md) table configuration. See [customizing the table used](/persistence/dynamodb/#usage-customizing-the-table-used).

Billing mode is not constrained by the persistence. Installers create the table as `PAY_PER_REQUEST`; a table created by the deployment process can use either mode. See [capacity planning](/persistence/dynamodb/capacity-planning.md) for choosing between them.

[Time-to-live](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/TTL.html) is required by the [outbox](/persistence/dynamodb/outbox.md). It is enabled by default so that the outbox can be enabled later without a migration. No secondary indexes are required.

## Create the table

The following commands use PowerShell line continuations; adjust them for other shells.

```ps
aws dynamodb create-table `
  --table-name NServiceBus.Storage `
  --attribute-definitions AttributeName=PK,AttributeType=S AttributeName=SK,AttributeType=S `
  --key-schema AttributeName=PK,KeyType=HASH AttributeName=SK,KeyType=RANGE `
  --billing-mode PAY_PER_REQUEST

aws dynamodb wait table-exists --table-name NServiceBus.Storage

aws dynamodb update-time-to-live `
  --table-name NServiceBus.Storage `
  --time-to-live-specification "Enabled=true,AttributeName=ExpiresAt"
```

Time-to-live can only be configured once the table is active, which is why the `wait table-exists` call separates the two commands.

When saga and outbox data are stored in separate tables, create each one with the same key schema. Only the outbox table needs time-to-live enabled.
