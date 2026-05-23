---
title: Endpoints and endpoint instances
summary: Defines the concepts of endpoint and endpoint instance.
reviewed: 2026-05-23
component: Core
redirects:
- nservicebus/endpoint
- nservicebus/hosting/send-only-endpoints
---

An _endpoint_ is a logical component that communicates with other components using [_messages_](/nservicebus/messaging). Each endpoint has an identifying name, contains a collection of [_message handlers_](/nservicebus/handlers/) and/or [_sagas_](/nservicebus/sagas/), and is deployed to a given _environment_ (e.g. development, testing, production).

## Endpoint instance

An _endpoint instance_ is a physical deployment of an endpoint. Each endpoint instance processes messages from an input queue which contains messages for the endpoint to process. Each endpoint has at least one endpoint instance. Additional endpoint instances may be added to [scale-out](/nservicebus/scaling.md) the endpoint.

## Logical endpoints

A collection of endpoint instances represents a single logical endpoint if and only if:

- The endpoint instances have the same name.
- The endpoint instances contain the same collection of handlers and/or sagas.
- The endpoint instances are deployed to the same environment.

## Send-only

A send-only endpoint is used when the only purpose is sending messages, and no message processing is required. Common use cases include websites, console applications, and Windows applications.

To configure an endpoint in send-only mode:

snippet: send-only-endpoint

Send-only endpoints do not initialize receiving infrastructure. The only additional configuration required when running in this mode is the destination when [sending a message](/nservicebus/messaging/send-a-message.md).
