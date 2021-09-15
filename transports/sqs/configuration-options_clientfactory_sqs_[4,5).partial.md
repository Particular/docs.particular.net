## Client

**Optional**

**Default**: `new AmazonSQSClient()`

By default the transport uses a parameterless constructor to build the SQS client. This overrides the default SQS client with a custom one.

**Example**: To use a custom client, specify:

snippet: ClientFactory