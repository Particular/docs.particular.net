---
title: Using Fully Qualified Domain Names
summary: How to use Fully Qualified Domain Names (FQDN) for MSMQ routing with NServiceBus
component: MsmqTransport
versions: '[4,)'
reviewed: 2021-05-26
redirects:
 - nservicebus/msmq/full-qualified-domain-name
---

By default, the MSMQ transport uses the local machine name, taken from `Environment.MachineName`, as the originator and reply-to address in messages. Windows can use NETBIOS to connect to the destination machine. Some deployment environments require the use of [Fully Qualified Domain Names (FQDN)](https://en.wikipedia.org/wiki/Fully_qualified_domain_name) to route messages correctly. The most common scenario is routing between machines in different domains.

To use the FQDN, [override the machine name](/nservicebus/hosting/override-machine-name.md)

## Routing

The [routing](/nservicebus/messaging/routing.md) configuration and any explicit queue destinations will also need to use the FQDN.
