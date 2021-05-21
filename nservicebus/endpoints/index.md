---
title: Endpoints
summary: Describe the concepts of Endpoint and Endpoint Instance.
reviewed: 2019-07-18
component: Core
related:
 - samples/endpoint-configuration
redirects:
- nservicebus/endpoint
---

An _endpoint_ is a logical entity that communicates with other endpoints via [_messaging_](/nservicebus/messaging). Each endpoint has an identifying name and contains a collection of [_message handlers_](/nservicebus/handlers/) and [_sagas_](/nservicebus/sagas/). An endpoint can be deployed to a number of machines and environments. Each deployment of an endpoint is an instance. Each endpoint instance processes messages from an input queue which contains messages for the endpoint to process.

It is common for each endpoint to have a single endpoint instance. As endpoints need to [scale-out](/nservicebus/architecture/scaling.md), additional endpoint instances can be added. This collection of endpoint instances still represents a single logical endpoint.
