---
title: Azure Service Bus Transport Sanitization
summary: Sanitization with Azure Service Bus and how it works
component: ASB
tags:
- Cloud
- Azure
- Transports
reviewed: 2016-06-23
---


## Azure Service Bus entity naming rules

Azure Service Bus transport works with the Messaging namespaces and operates on four types of entities: queues, topics, subscriptions, and rules. Entities have path and name rules applied to:

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


When sanitization strategy is specified, the validation settings can be overridden. The validation settings determine how entity names are validated. They should correspond to the actual validation rules in the configured Azure Service Bus namespace. The rules implementations vary depending on the namespace type, and are changing over time (in some cases without notice and update of the [relevant MSDN documentation](https://azure.microsoft.com/en-us/documentation/articles/service-bus-quotas/)). The default settings align with the recently created Standard namespaces.


## Sanitization

Sanitization is an operation that is performed on an entity path or name to ensure the broker can operate on the entity, and no exception is thrown. Sanitization can be performed manually or by the Azure Service Bus transport.

To perform manual sanitization, inspect entity name for invalid characters and number of characters. For automated sanitization, the transport allows configuration outlined below.


## Automated sanitization


### Versions 6 and below

All entities treated the same for allowed characters. Only letters, numbers, periods (`.`), hyphens (`-`), and underscores (`_`) were allowed. Maximum length was predefined by the transport.

In Version 6.4.x [Naming Conventions](/nservicebus/azure-service-bus/naming-conventions.md) were added to allow custom sanitization of queues, topics, and subscriptions.


### Versions 7 and above

By default, the transport uses the `ThrowOnFailedValidation` sanitization strategy. This strategy allows sanitization rules to be specified that remove invalid characters and hashing algorithm to shorten entity path/name that is exceeding maximum length. For an invalid entity path/name, an exception is thrown. Validation rules can be adjusted by providing custom validation rules per entity type.

snippet: asb-ThrowOnFailedValidation-sanitization-overrides

Where `ValidationResult` provides the following

 * Characters are valid or not
 * Length is valid or not

To customize sanitization for some of the entities, `ValidateAndHashIfNeeded` strategy can be used. This strategy allows to specify sanitization rules to remove invalid characters and hashing algorithm to shorten entity path/name that is exceeding maximum length.

NOTE: `ValidateAndHashIfNeeded` is using validation rules to determine what needs to be sanitized. First step, invalid characters are removed. Second step, hashing is applied if length is still exceeding the maximum allowed length.

snippet: asb-ValidateAndHashIfNeeded-sanitization-overrides

In cases where an alternative sanitization is required, a custom sanitization can be applied.

snippet: asb-custom-sanitization

Custom sanitization strategy definition:

snippet: asb-custom-sanitization-strategy

If the implementation of a sanitization strategy requires configuration settings, these settings can be accessed using [Dependency Injection](/nservicebus/containers/) to access an instance of `ReadOnlySettings`.

snippet: custom-sanitization-strategy-with-settings


### Backward compatibility with versions 6 and below

To remain backward compatible with endpoints versions 6 and below, endpoints version 7 and above should be configured to perform sanitization based on version 6 and below rules. The following custom topology will ensure entities are sanitized in a backwards compatible manner.

snippet: asb-backward-compatible-custom-sanitiaztion-strategy


## Future consideration prior to using sanitization

When implementing custom sanitization, consider factors such as readability and discover-ability. Things to consider:

 * Truncated long entity names could conflict. 
 * Hashed entity names could lead to difficult names to use during production troubleshooting or debugging.
 * Sanitized entity names stay in the system and cannot be replaced until no longer used.


### Possible ways to avoid sanitization

 * Define endpoint name as short and meaningful.
 * Avoid message definitions in deep-nested namespaces.
 * Keep event names descriptive and short.
