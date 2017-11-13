
### DisableConnectionCachingForSends

By default, caching is set to true and instructs MSMQ to cache connections to a remote queue and re-use them as needed instead of creating new connections for each message. This API allows connection caching to be turned off. However doing so will negatively impact the message throughput in most scenarios. 
 
snippet: disable-connection-caching-for-sends

