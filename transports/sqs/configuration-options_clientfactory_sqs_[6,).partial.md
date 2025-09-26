## SQS Client

**Optional**

**Default**: `new AmazonSQSClient()`

By default the transport uses a parameterless constructor to build the SQS client. This overrides the default SQS client with a custom one.

**Example**: To use a custom client, specify:

snippet: ClientFactory

> [!NOTE]
> If a custom SQS client is provided, it will not be disposed of when the endpoint is stopped.

## SNS Client

**Optional**

**Default**: `new AmazonSimpleNotificationServiceClient()`

By default the transport uses a parameterless constructor to build the SNS client. This overrides the default SNS client with a custom one.

**Example**: To use a custom client, specify:

snippet: SnsClientFactory

> [!NOTE]
> If a custom SNS client is provided, it will not be disposed of when the endpoint is stopped.
