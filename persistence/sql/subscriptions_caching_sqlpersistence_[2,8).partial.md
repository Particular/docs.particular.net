## Caching

The storage of subscription information is required for [unicast transports](/transports/types.md#unicast-only-transports).

Subscription information can be cached for a given period of time so that it does not have to be queried from the database every single time an event is being published. The longer the cache period is, however, the higher the chance that new subscribers miss some events. This can happen when a subscription message arrives after the subscription information has been loaded into memory. These scenarios mean that there is no good default value for the subscription caching period; it has to be specified by the user. 

In systems where subscriptions are static, the caching period can be relatively long. To configure it, use the following API:

snippet: subscriptions_CacheFor

In systems where events are subscribed and unsubscribed regularly (e.g. desktop applications unsubscribe when shutting down), it makes sense to keep the caching period short or to disable the caching altogether:

snippet: subscriptions_Disable
