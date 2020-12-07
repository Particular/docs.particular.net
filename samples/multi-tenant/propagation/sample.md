---
title: Propagating Tenant Information to Downstream Endpoints
summary: How to configure the pipeline to automatically propagate tenant information to downstream endpoints
reviewed: 2020-12-07
component: Core
related:
- nservicebus/pipeline/manipulate-with-behaviors
---


This sample shows how to configure the NServiceBus pipeline to automatically propagate tenant information to downstream endpoints. The sample assumes that the tenant information is passed as a message header.


## Code walk-through


### Attaching tenant information to the messages

In most cases the best way to attach tenant information to messages is with a custom message header. The following code demonstrates how to set a custom `tenant_id` header.

snippet: SetTenantId


### Creating behaviors

Two behaviors are required to propagate the tenant information.


#### Retrieving and storing the tenant information

The first behavior is responsible for extracting the tenant information from the incoming message header and placing it in the pipeline execution context bag. This behavior executes as part of the message receive pipeline.

snippet: StoreTenantId


#### Propagating the tenant information to the outgoing messages

The second behavior is responsible for attaching the tenant information header(s) to outgoing messages based on the pipeline context. This behavior executes as part of the message send pipeline.

snippet: PropagateTenantId


### Registering the behaviors

The following code is needed to register the created behaviors in the pipeline.

snippet: configuration


### Message handlers

The `OrderPlaced` message handler in the Sales endpoint publishes an `OrderAccepted` event.

snippet: message-handler

In addition to that, both `OrderAccepted` and `OrderPlaced` message handlers log the tenant ID header value extracted from the incoming message.


## Running the sample

Run the sample. Once running, in the Client console press any key to send messages. Note that the logged tenant ID is the same in both the Sales and Billing endpoints.
