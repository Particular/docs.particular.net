---
title: Endpoints
summary: Describe the concepts of Endpoint and Endpoint Instance.
reviewed: 2016-03-17
tags:
- Endpoint
- Endpoint Instance
related:
 - samples/endpoint-configuration
redirects:
- nservicebus/endpoint
---

An Endpoint is a logical entity that communicates with other Endpoints via [Messaging](/nservicebus/messaging). Each Endpoint has an identifying name and contains a collection of [Message Handlers](/nservicebus/handlers/) and [Sagas](/nservicebus/sagas/). An Endpoint can be deployed to a number of machines and environments. Each deployment of an endpoint is an instance. Each Endpoint Instance has an input queue which contains messages for the Endpoint Instance to process. 

It is common for each Endpoint to have a single Endpoint Instance. As Endpoints need to [scale-out](/nservicebus/scalability-and-ha/scale-out.md), additional Endpoint Instances can be added. This collection of Endpoint Instances still represents a single logical Endpoint.