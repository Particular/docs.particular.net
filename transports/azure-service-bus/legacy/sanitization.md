---
title: Sanitization
component: ASB
reviewed: 2019-10-01
redirects:
 - nservicebus/azure-service-bus/sanitization
 - transports/azure-service-bus/sanitization
---

include: legacy-asb-warning


## Azure Service Bus entity naming rules

Azure Service Bus transport works with the Messaging namespaces and operates on four types of entities: queues, topics, subscriptions, and rules. Entities have their paths and names derived from NServiceBus endpoint and event names. Entities have path and name rules applied to:

 1. Allowed characters
 1. Maximum length

NOTE: Only queues and topics can have a path.

When entity name does not follow the rules, an exception is raised by the broker and transport fails to start. Paths and names need to be validated and adhere to the Azure Service Bus service rules.

| Entity Type  | Valid Characters | Path / Name Maximum Length |
|--------------|------------------|----------------------------|
| Queue        | Letters, numbers, periods (`.`), hyphens (`-`), underscores (`_`), and forward slashes (`/`). | 260 |
| Topic        | Letters, numbers, periods (`.`), hyphens (`-`), underscores (`_`), and forward slashes `(/`). | 260 |
| Subscription | Letters, numbers, periods (`.`), hyphens (`-`), and underscores (`_`). | 50  |
| Rule         | Letters, numbers, periods (`.`), hyphens (`-`), and underscores (`_`). | 50  |


When sanitization strategy is specified, the validation settings can be overridden. The validation settings determine how entity names are validated. They should correspond to the validation rules in the configured Azure Service Bus namespace. The rules implementations vary depending on the namespace type, and are changing over time (in some cases without notice and update of the [relevant MSDN documentation](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas)). The default settings align with the recently created Standard namespaces.


## Sanitization

Sanitization is an operation that is performed on an entity path or name to ensure the broker can operate on the entity, and no exception is thrown. Sanitization can be performed manually or by the Azure Service Bus transport.

To perform manual sanitization, inspect entity name for invalid characters and number of characters. For automated sanitization, the transport allows configuration outlined below.


## Automated sanitization

partial: auto


## Future consideration prior to using sanitization

When implementing custom sanitization, consider factors such as readability and discover-ability. Things to consider:

 * Truncated long entity names could conflict.
 * Hashed entity names could lead to difficult names to use during production troubleshooting or debugging.
 * Sanitized entity names stay in the system and cannot be replaced until no longer used.
 * Composition strategy needs to work in conjunction with custom sanitization.
     * If `HierarchyComposition` is used, the generated path must be shared with the custom sanitization strategy and be exempted from sanitization.


### Possible ways to avoid sanitization

 * Define endpoint name as short and meaningful.
 * Avoid message definitions in deep-nested namespaces.
 * Keep event names descriptive and short.