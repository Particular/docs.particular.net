---
title: Specify Endpoint Name
summary: There are many ways to specify the endpoint name.
reviewed: 2016-03-17
tags:
- Endpoint Name
- Conventions
- Input queue
redirects:
- nservicebus/how-to-specify-your-input-queue-name
- nservicebus/messaging/specify-input-queue-name
- nservicebus/endpoint/specify-input-queue-name
- nservicebus/endpoints/specify-input-queue-name
---


## When using the configuration API

NOTE: these approaches can also be used from [NServiceBus Host](/nservicebus/hosting/nservicebus-host/) via the use of `IConfigureThisEndpoint`.

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

NOTE: Note Only use code OR commandline/installation parameters.

## Input queue

By default the endpoint name is used as a basis for generating the input queue name. This default could be overridden in versions 5 and above using following API

snippet:InputQueueName

Other endpoints that send messages to the one with overridden input queue name need to be aware of that override.

snippet:InputQueueOverrideRouting

In Version 5 and previous versions the routing configuration references the overridden input queue directly. In Version 6 and above the logical routing works on endpoint name level and a separate API call is required to make that endpoint aware of the customized input queue of `MyEndpoint`.

NOTE: In Version 6 and above the same API call is used to override the default translation of endpoint name to transport address (input queue name) for the endpoint's own input queue as well as for any destination endpoint. To learn more about transport address generation in Version 6 and above go to the [routing](/nservicebus/messaging/routing.md) documentation.