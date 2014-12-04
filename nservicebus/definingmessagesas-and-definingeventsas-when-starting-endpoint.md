---
title: DefiningMessagesAs and DefiningEventsAs When Starting Endpoint
summary: To fix an exception when starting an endpoint with DefiningMessagesAs, include your default namespace.
tags: []
---

When defining an conventions the following way:

<!-- import UnobtrusiveConventionsFaqError -->

A FATAL NServiceBus.Hosting.GenericHost exception is thrown: `Exception when starting endpoint.`.

The reason is that NServiceBus itself uses namespaces that end with "Messages". To fix the error include your default namespace; for example:


<!-- import UnobtrusiveConventionsFaqFix -->



