---
title: Specify Endpoint Name
summary: There are many ways to specify the endpoint name.
reviewed: 2016-03-17
tags:
- Endpoint Name
- Conventions
redirects:
- nservicebus/how-to-specify-your-input-queue-name
- nservicebus/messaging/specify-input-queue-name
- nservicebus/endpoint/specify-input-queue-name
- nservicebus/endpoints/specify-input-queue-name
---


## When using the configuration API

Define a convention in the endpoint initialization code using this:

snippet:EndpointNameCode


## When using the NServiceBus.Host.exe


### Namespace convention

When using NServiceBus.Host, the namespace of the class implementing `IConfigureThisEndpoint` will be used as the endpoint name as the default convention. In the following example the endpoint name when running NServiceBus host becomes `MyServer`. This is the recommended way to name a endpoint. Also this emphasizes convention over configuration approach.

snippet:EndpointNameByNamespace


### EndpointName attribute

Set the endpoint name using the `[EndpointName]` attribute on the endpoint configuration.

NOTE: This will only work when using [NServiceBus host](/nservicebus/hosting/nservicebus-host/).

snippet: EndpointNameByAttribute


### Installation parameter

When specifying an explicit service name when installing the NServiceBus host, this is used as the endpoint name: `/serviceName:"MyEndpoint"`.


### Command-line parameter

Specify a endpoint name when running the NServiceBus host: `/endpointName:"MyEndpoint"`.

NOTE: Only use code **OR** commandline/installation parameters.


## Input queue

By default the endpoint's input queue name is the same as endpoint's name. The input queue name can be overridden in Versions 5 and above using the following API:

snippet:InputQueueName

Additionally, the actual queue name has to be passed to the endpoints that want to communicate with it:

snippet:InputQueueOverrideRoutingXml

snippet:InputQueueOverrideRouting

In Versions 5 and below the overridden input queue is passed directly to the routing configuration. In Versions 6 and above the logical routing works on an endpoint name level. The endpoint name is passed to the routing configuration, and a separate API call is required to specify the name of its input queue.

NOTE: In Versions 6 and above the same API is used to override the default translation of the endpoint name to the transport address (input queue name) for the endpoint's own input queue, as well as for any destination endpoints. To learn more about transport address generation in Versions 6 and above refer to the [routing](/nservicebus/messaging/routing.md) documentation.
