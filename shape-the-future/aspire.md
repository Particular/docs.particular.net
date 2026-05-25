---
title: NServiceBus and Aspire
summary: How to use NServiceBus with Aspire
reviewed: 2026-05-21
---

[Aspire](https://aspire.dev/) is a code-first orchestration and observability layer for distributed applications.

We have a [custom Aspire integration for the Particular Platform](/platform/aspire/) that runs ServiceControl, ServicePulse, and a managed RavenDB persistence instance inside an Aspire AppHost alongside NServiceBus endpoints.

The integration is being released in stages as more transports and persisters are added; see the [supported components matrix](/platform/aspire/index.md#supported-components) on the integration page for the current status.

If you are using NServiceBus with Aspire today, [tell us what's missing](https://github.com/Particular/NServiceBus/issues/6941).
