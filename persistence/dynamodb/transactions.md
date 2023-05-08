---
title: DynamoDB persistence transactions
summary: How to use transactions with DynamoDB persistence
component: DynamoDB
reviewed: 2023-03-16
related:
- persistence/dynamodb
---

The [DynamoDB TransactWriteItems](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/transactions.html) API is used to commit Outbox and Saga changes in a single transaction. Message handlers can add further operations to this transaction, via the synchronized session:

snippet: DynamoDBSynchronizedSession

Transactions can contain a maximum of 100 operations. This limit is shared with operations enlisted by NServiceBus. Each saga will use one operation. Outbox will use `1 + <amount of outgoing messages>` operations.

## Mapping

The AWS SDK provides a built-in mechanism to map attribute values to custom types and vice-versa when using the [`DynamoDBContext`](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DotNetDynamoDBContext.html). Unfortunately the `DynamoDBContext` class does not support transactional writes and comes with built-in assumptions on how classes should be annotated. On top of that, it tries to validate table structures in a synchronous and blocking way that can degrade the systems throughput. Additionally, the validation logic can make it difficult to use custom types stored together with the saga types in the same table when the single table design is preferred. To circumvent some of these issues, the persistence provides a custom mapper that supports all built-in data types and uses `System.Text.Json` under the covers.

The following snippets shows how to use the mapper function together with the synchronized storage to map attribute values to custom types and back.

snippet: DynamoDBMapperUsageWithoutKeyMappingCustomType

When the custom type doesn't contain an explicit mapped property for the partition and sort keys, it is crucial that both keys are added to the mapped attribute values before adding it to the storage session as shown below.

snippet: DynamoDBMapperUsageWithoutKeyMapping

It is possible to also map the partition and the sort key by annotating a property with the corresponding partition and sort key name expressed with a `JsonPropertyName` attribute.

snippet: DynamoDBMapperUsageWithKeyMappingCustomType

with this in place the custom types can be mapped without further modification

snippet: DynamoDBMapperUsageWithKeyMapping

### Supported data types

The mapper supports the following [data types](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.NamingRulesDataTypes.html#HowItWorks.DataTypes):

- Number
- String
- Binary
- Boolean
- Null
- List
- Map
- Sets

The mapper is closely aligned with the DynamoDB v2 type mapping behavior in the AWS .NET SDK.

Binaries need to be expressed as `MemoryStream`. For efficiency reasons the mapper does not copy the memory stream but directly adds a reference to the original `MemoryStream` into the attribute value dictionary and vice versa.

Sets are automatically used when the type has properties of type `ISet<>`.

Hierarchical objects are serialized into maps.

## DynamoDBContext

It is possible to combine [`DynamoDBContext`](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DotNetDynamoDBContext.html) usage together with the synchronized storage but there are a number of things that need to be taken into account.

- Custom types that use renamed attribute names need to have a corresponding `JsonPropertyName` attribute
- In order to participate in the synchronized storage transaction `SaveChangesAsync` **must not be called**

Following up on the previous example, when mapping the `Customer` type with the `DynamoDBContext`, the `CustomerId` and the `CustomerPreferred` properties would need the `DynamoDBHashKey` or `DynamoDBProperty` attributes, and the `JsonPropertyName` attribute.

snippet: DynamoDBMapperContextUsageCustomType

with mapping in place, loaded customers can be mapped into the storage session and will only be modified when the synchronized storage commits its transaction.

snippet: DynamoDBMapperContextUsage

## Testing

When [unit testing](/samples/unit-testing/) a message handler, the `TestableDynamoDBSynchronizedStorageSession` class can be used:

snippet: DynamoDBTestingSessionUsage
