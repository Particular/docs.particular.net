---
title: Specify Your Input Queue
summary: There are many ways to specify the input endpoint name.
tags:
- Endpoint Name
- Conventions
redirects:
- nservicebus/how-to-specify-your-input-queue-name
---

## When using the configuration API

NOTE: these approaches can also be used from NServiceBus Host via the use of `IConfigureThisEndpoint`.

You can define your own convention in the endpoint initialization code using this

snippet:EndpointNameCode


## When using the NServiceBus.Host.exe 


### Namespace convention

When using NServiceBus.Host, the namespace of the class implementing `IConfigureThisEndpoint` will be used as the endpoint name as the default convention. In the following example the endpoint name when running NServiceBus host becomes `MyServer`. This is the recommended way to name a endpoint. Also this emphasizes convention over configuration approach.

snippet:EndpointNameByNamespace

### EndpointName attribute

You can set the endpoint name using the `[EndpointName]` attribute on your endpoint configuration.

NOTE: This will only work when using [NServiceBus host](/nservicebus/hosting/nservicebus-host/).

snippet: EndpointNameByAttribute


### Installation parameter

If you specify a explicit service name when installing the NServiceBus host, this is used as the endpoint name: `/serviceName:"MyEndpoint"`.


### Command-line parameter

You can specify a endpoint name when running the NServiceBus host: `/endpointName:"MyEndpoint"`.

NOTE: Note you can only use code OR commandline/installation parameters.