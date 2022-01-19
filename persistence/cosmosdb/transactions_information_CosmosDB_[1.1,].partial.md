A configuration API has been introduced as of version 1.1 and above. This configuration API provides the ability to specify the partition and Container used per message using message headers or the message contents.

Using message headers only has the advantage being able to identify the partition or `Container` before the [outbox](/nservicebus/outbox) logic is executed, which allows the outbox feature to work entirely as intended.

In the case where the partition or Container cannot be identified using only the message headers, the message contents can be used. This works because the Cosmos DB persistence introduces additional outbox logic to locate the outbox record and bypass the remaining message processing pipeline at a later stage of processing.

## Specifying the `Container` partition to use for the transaction

Transactions can only be executed within a single `Container` partition. The `PartitionKey` must be specified for transactions to work in Azure Cosmos DB.

### Using message header values

A single message header value can be used to specify the `PartitionKey` for the partition:

snippet: ExtractPartitionKeyFromHeaderSimple

Multiple message header values can also be used. Additionally overloads exist that allow a state object to be provided and passed when the extractor is called to avoid unnecessary allocations:

snippet: ExtractPartitionKeyFromHeadersExtractor

A custom class that implements `IPartitionKeyFromHeadersExtractor` can be implemented to specify the `PartitionKey` using message headers:

snippet: CustomPartitionKeyFromHeadersExtractor

The `IPartitionKeyFromHeadersExtractor` implementation can be registered via dependency injection or configured via the API:

snippet: ExtractPartitionKeyFromHeadersCustom

Besides those API methods shown here, additional overloads are available for extracting `PartitionKey`.

### Using the message contents

The message contents can be accessed to specify the `PartitionKey` of the partition for the transaction:

snippet: ExtractPartitionKeyFromMessageExtractor

A custom class that implements `IPartitionKeyFromMessageExtractor` can be implemented that can access the message contents and headers to specify the partition to use for the transaction:

snippet: CustomPartitionKeyFromMessageExtractor

The `IPartitionKeyFromMessageExtractor` implementation can be registered via dependency injection or configured using the API:

snippet: ExtractPartitionKeyFromMessageCustom

Additional overloads are available for extracting `PartitionKey`.

## Specifying the `Container` to use for the transaction

The Container to use can be specified by defining a default container:

include: defaultcontainer

Optionally, the `Container` to use can specified during message processing by providing the `Container` name and partition key path using the `ContainerInformation` object.

include: containeroverride

### Using message header values

A single message header value can be used to specify the container:

snippet: ExtractContainerInfoFromHeader

Multiple message header values can also be used. Additionally overloads exist that allow a state object to be passed when the extractor is called to avoid unnecessary allocations:

snippet: ExtractContainerInfoFromHeaders

A custom class that implements `IContainerInformationFromHeadersExtractor` can be implemented to specify the `Container` using message headers:

snippet: CustomContainerInformationFromHeadersExtractor

The `IContainerInformationFromHeadersExtractor` implementation can be registered via dependency injection or configured using the API:

snippet: ExtractContainerInfoFromHeadersCustom

Besides those API methods shown here, additional overloads are available for extracting `ContainerInformation` from headers.

### Using the message contents

The message contents can be accessed to specify the container to use for the transaction:

snippet: ExtractContainerInfoFromMessageExtractor

A custom class that implements `IContainerInformationFromMessagesExtractor` can be implemented that makes use of the messages and headers to specify the container to use for the transaction:

snippet: CustomContainerInformationFromMessageExtractor

The `IContainerInformationFromMessagesExtractor` implementation can be registered via dependency injection or configured using the API:

snippet: ExtractContainerInfoFromMessageCustom

Additional overloads are available for extracting `ContainerInformation` from the message.