## Client Factory

**Optional**

**Default**: `() => new AmazonSQSClient()`.

This overloads the default SQS client factory with a custom factory creation delegate.

**Example**: To use a custom factory, specify:

snippet: ClientFactory