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

### Defined in code

Set the endpoint name using the `DefineEndpointName(name)` extension method on the endpoint configuration.

snippet:EndpointNameInCode

### EndpointName attribute

Set the endpoint name using the `[EndpointName]` attribute on the endpoint configuration.

NOTE: This will only work when using [NServiceBus host](/nservicebus/hosting/nservicebus-host/).

snippet: EndpointNameByAttribute


### Installation parameter

When specifying an explicit service name when installing the NServiceBus host, this is used as the endpoint name: `/serviceName:"MyEndpoint"`.


### Command-line parameter

Specify a endpoint name when running the NServiceBus host: `/endpointName:"MyEndpoint"`.

NOTE: Only use code **OR** commandline/installation parameters.

## When using NServiceBus.Hosting.Azure.HostProcess.exe

### Defined in code

Set the endpoint name using the `DefineEndpointName(name)` extension method on the endpoint configuration.

snippet:EndpointNameInCodeForAzureHost

## Input queue

By default the endpoint's input queue name is the same as endpoint's name. The input queue name can be overridden in Versions 5 and above using the following API:

snippet:InputQueueName

Note: Changing the input queue may also require adjusting the routing configuration for senders to send messages to the renamed queue. See the [routing documentation](/nservicebus/messaging/routing.md) for further details about routing configuration.

