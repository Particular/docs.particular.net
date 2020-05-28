---
title: Specify Endpoint Name
summary: Describes the ways in which to specify an endpoint name
reviewed: 2019-12-25
component: Core
redirects:
- nservicebus/how-to-specify-your-input-queue-name
- nservicebus/messaging/specify-input-queue-name
- nservicebus/endpoint/specify-input-queue-name
- nservicebus/endpoints/specify-input-queue-name
---


Define the endpoint name at initialization time:

snippet: EndpointNameCode

See also:
 
 * [Specify Endpoint Name in NServiceBus Host](/nservicebus/hosting/nservicebus-host/#endpoint-configuration-endpoint-name)
 * [Configure an Endpoint in Azure Cloud Services](/nservicebus/hosting/cloud-services-host/configuration.md#configuring-an-endpoint)


## Input queue

By default, the endpoint's input queue name is the same as endpoint's name.

The input queue name can be overridden:

snippet: InputQueueName

Note: Changing the input queue may also require adjusting the routing configuration for senders to send messages to the renamed queue. See [routing configuration](/nservicebus/messaging/routing.md).