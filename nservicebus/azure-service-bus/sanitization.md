---
title: Azure Service Bus Transport Sanitization
summary: Sanitization with Azure Service Bus and how it works
component: ASB
tags:
- Cloud
- Azure
- Transports
reviewed: 2016-05-02
---


## Azure Service Bus entity naming rules

Azure Service Bus transport works with the Messaging namespaces and operates on four types of entities: queues, topics, subscriptions, and rules. Entities have path and name rules applied to:

 1. Allowed characters
 1. Maximum length

NOTE: Only queues and topics can have a path.

When entity name does not follow the rules, an exception is raised by the broker and transport fails to start. Paths and names need to be validated and adhere to the Azure Service Bus service rules.

| Entity Type  | Valid Characters | Path / Name Maximum Length |
|--------------|------------------|----------------------------|
| Queue        | Letters, numbers, periods (.), hyphens (-), underscores (_), and forward slashes (/). | 260 |
| Topic        | Letters, numbers, periods (.), hyphens (-), underscores (_), and forward slashes (/). | 260 |
| Subscription | Letters, numbers, periods (.), hyphens (-), and underscores (_). | 50  |
| Rule         | Letters, numbers, periods (.), hyphens (-), and underscores (_). | 50  |


## Sanitization

Sanitization is an operation on entity path or name to ensure the broker can operate on the entity, and no exception is thrown. Sanitization can be performed manually or by the Azure Service Bus transport.

To perform manual sanitization, inspect entity name for invalid characters and number of characters. For automated sanitization, the transport allows configuration outlined below.


## Automated sanitization


### Versions 6 and below

All entities treated the same for allowed characters. Only letters, numbers, periods (.), hyphens (-), and underscores (_) were allowed. Maximum length was predefined by the transport.

In Version 6.4.x [Naming Conventions](/nservicebus/azure-service-bus/native-integration.md) were added to allow custom sanitization of queues, topics, and subscriptions.


### Versions 7 and above

By default, the transport does not sanitize entities. Instead, it passes entity paths/names as-is to the broker.

In cases where entities are too long, and there is no way to shorten those, custom sanitization can be applied on the transport level to ensure names adhere to the Azure Service Bus service rules.

snippet: asb-custom-sanitization

Custom sanitization strategy definition:

snippet: asb-custom-sanitization-strategy

When using `EndpointOrientedTopology`, the transport should be configured to use `EndpointOrientedTopologySanitization` sanitization strategy for backward compatibility with endpoints version 6 and below.

snippet: asb-endpointorientedtopology-sanitization


## Future consideration prior to using sanitization

When implementing a custom sanitization, consider factors such as readability and discover-ability. Things to consider:

 * Truncated long entity names could conflict. 
 * Hashed entity names could lead to difficult names to use during production troubleshooting or debugging.
 * Sanitized entity names stay in the system and cannot be replaced until no longer used.


### Possible ways to avoid sanitization

 * Define endpoint name as short and meaningful.
 * Avoid message definitions in deep-nested namespaces.
 * Keep event names descriptive and short.