## Enabling non-durable messaging

This feature can be enabled in two ways


### For specific message types

#### Via an Express attribute

A message can be configured to be non-durable via the use of an `[ExpressAttribute]`.

snippet: ExpressMessageAttribute


#### Using the configuration API

A subset of messages can be configured to be non-durable by using a convention.

snippet: ExpressMessageConvention


### Global for the endpoint

Allows messages to be non-durable via the configuration API.

snippet: set-to-non-durable
