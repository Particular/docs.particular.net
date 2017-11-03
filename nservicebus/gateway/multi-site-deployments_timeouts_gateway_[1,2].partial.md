## Transaction timeouts

Transmitting messages to remote sites will by default use the default transaction timeout of the underlying transport. A custom timeout value can be configured as follows:

snippet: CustomGatewayTimeoutConfig

NOTE: The value must be a valid [`System.TimeSpan`](https://msdn.microsoft.com/en-us/library/se73z7b9)