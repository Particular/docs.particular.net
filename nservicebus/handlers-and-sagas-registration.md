---
title: Registering Handlers and Sagas
summary: How to register message handlers and sagas with an NServiceBus endpoint.
component: Core
reviewed: 2026-04-27
---

Registration tells an endpoint which message handlers and sagas to include. NServiceBus supports explicit registration, recommended for new endpoints, and automatic discovery via assembly scanning, useful for plugin or dynamic discovery scenarios.

partial: registration-content