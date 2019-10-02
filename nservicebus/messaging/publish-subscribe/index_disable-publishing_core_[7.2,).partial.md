##### Disabling publishing

By default, NServiceBus requires a subscription persister to be configured. If an endpoint does not publish any events, the publish functionality can be disabled to avoid providing a subscription persister:

snippet: DisablePublishing