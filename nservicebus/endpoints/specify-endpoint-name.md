---
title: Specify Endpoint Name
summary: There are many ways to specify the endpoint name.
reviewed: 2016-08-24
component: Core
tags:
- Convention
redirects:
- nservicebus/how-to-specify-your-input-queue-name
- nservicebus/messaging/specify-input-queue-name
- nservicebus/endpoint/specify-input-queue-name
- nservicebus/endpoints/specify-input-queue-name
---


Define the endpoint name at initialization time using:

snippet: EndpointNameCode

See also:
 
 * [Specify Endpoint Name in NServiceBus Host](/nservicebus/hosting/nservicebus-host/#endpoint-configuration-endpoint-name)
 * [Configure an Endpoint in Azure Cloud Services](/nservicebus/hosting/cloud-services-host/configuration.md#configuring-an-endpoint)


## Input queue

By default the endpoint's input queue name is the same as endpoint's name.

partial: inputqueuename