## Transaction timeouts

Transmitting messages to remote sites will use the default transaction timeout of the underlying transport. To override this, a custom timeout value can be configured:

snippet: GatewayCustomTransactionTimeoutConfig

NOTE: The value must be a valid [`System.TimeSpan`](https://msdn.microsoft.com/en-us/library/se73z7b9).