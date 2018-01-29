---
title: Install Custom Checks Plugin
summary: Enabling custom endpoint instance monitoring by installing the Custom Checks plugin
reviewed: 2018-01-26
component: CustomChecks
versions: 'CustomChecks:*'
---

To install the Custom Checks plugin into an endpoint add the following to the endpoint configuration:

snippet: CustomCheckNew_Enable

It may not make sense to enable the custom checks plugin in all environments. For instance, a development environment may not have a running ServiceControl instance to consume custom check messages. In these cases, enable the plugin conditionally, based on an environment variable or configuration setting.

The `timeToLive` (TTL) parameter is optional and defaults to four times the interval for periodic checks or infinite for one-time checks. Some queue systems (e.g. MSMQ) handle TTL proactively by removing timed out messages from the queues. Others do it only when a message is about to be received. Running the CustomChecks without ServiceControl installed may cause the destination queue to grow infinitely and consume all available system resources.
