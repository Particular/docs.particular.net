## Gateway deduplication

INFO: It is advised to use the [in-memory persister that is part of the gateway package](/persistence/non-durable/gateway-deduplication.md).

The in-memory gateway deduplication persistence uses an LRU cache. By default this cache can contain up to 10,000 items. The maximum size can be changed using the following API.

snippet: GatewayDeduplicationCacheSize
