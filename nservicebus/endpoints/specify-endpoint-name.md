---
title: Specify Endpoint Name
summary: There are many ways to specify the endpoint name.
reviewed: 2016-08-24
component: Core
tags:
- Endpoint Name
- Conventions
redirects:
- nservicebus/how-to-specify-your-input-queue-name
- nservicebus/messaging/specify-input-queue-name
- nservicebus/endpoint/specify-input-queue-name
- nservicebus/endpoints/specify-input-queue-name
---


Define the endpoint name at initialization time using:

snippet:EndpointNameCode

See also:
 
 * [Specify Endpoint Name in NServiceBus Host](/nservicebus/hosting/nservicebus-host/#specify-endpoint-name)
 * [Specify Endpoint Name in Azure Cloud Services](/nservicebus/hosting/cloud-services-host/#specify-endpoint-name)


## Input queue

By default the endpoint's input queue name is the same as endpoint's name.

partial:inputqueuename