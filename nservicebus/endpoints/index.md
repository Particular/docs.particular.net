---
title: Endpoints and endpoint instances
summary: Defines the concepts of endpoint and endpoint instance.
reviewed: 2021-06-29
component: Core
related:
 - samples/endpoint-configuration
redirects:
- nservicebus/endpoint
---

An _endpoint_ is a logical component that communicates with other components using [_messages_](/nservicebus/messaging). Each endpoint has an identifying name, contains a collection of [_message handlers_](/nservicebus/handlers/) and/or [_sagas_](/nservicebus/sagas/), and is deployed to a given _environment_ (e.g. development, testing, production). 

An _endpoint instance_ is a physical deployment of an endpoint. Each endpoint instance processes messages from an input queue which contains messages for the endpoint to process. Each endpoint has at least one endpoint instance. Additional endpoint instances may be added to [scale-out](/nservicebus/architecture/scaling.md) the endpoint. A collection of endpoint instances represents a single endpoint if and only if:

- The endpoint instances have the same name.
- The endpoint instances contain the same collection of handlers and/or sagas.
- The endpoint instances are deployed to the same environment.
