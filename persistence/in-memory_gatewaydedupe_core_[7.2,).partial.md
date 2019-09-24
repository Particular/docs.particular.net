## Gateway deduplication

The in-memory gateway deduplication persistence uses an LRU cache. By default this cache can contain up to 10,000 items. The maximum size can be changed using the following API.

snippet: GatewayDeduplicationCacheSize
