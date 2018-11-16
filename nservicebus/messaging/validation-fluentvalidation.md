---
title: FluentValidation message validation
summary: Validate message using FluentValidation.
reviewed: 2018-10-25
component: FluentValidation
related:
 - samples/fluent-validation
---

Uses [FluentValidation](https://github.com/JeremySkinner/FluentValidation) to validate incoming and outgoing messages.

FluentValidation message validation can be enabled using the following:

snippet: FluentValidation

include: validationexception

By default, incoming and outgoing messages are validated.

To disable for incoming messages use the following:

snippet: FluentValidation_disableincoming

To disable for outgoing messages use the following:

snippet: FluentValidation_disableoutgoing

include: validationoutgoing

Messages can then have an associated [validator](https://github.com/JeremySkinner/FluentValidation/wiki/b.-Creating-a-Validator):

snippet: FluentValidation_message


## Accessing the current pipeline context

In some cases a validator may need to use data from the current message context.

The current message context can be accessed via two extension methods:

 * The current [message headers](/nservicebus/messaging/headers.md) can be accessed via `FluentValidationExtensions.Headers(this CustomContext customContext)`
 * The current `ContextBag` can be accessed via `FluentValidationExtensions.ContextBag(this CustomContext customContext)`.

snippet: FluentValidation_ContextValidator


## Validator scanning

Validators are registered and resolved using [dependency injection](/nservicebus/dependency-injection/). Assemblies can be added for validator scanning using either a generic Type, a Type instance, or an assembly instance.

snippet: FluentValidation_AddValidators

Validator lifecycle can either be per endpoint ([Single instance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)):

snippet: FluentValidation_EndpointLifecycle

Or [instance per unit of work](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work):

snippet: FluentValidation_UnitOfWorkLifecycle

The default lifecycle is per endpoint.

By default, there are two exception scenarios when adding validators. An exception will be thrown if:

 * No validators are found in an assembly that is scanned.
 * Any non-public validators are found in an assembly that is scanned.

These exception scenarios can be excluded using the following:

snippet: FluentValidation_IgnoreValidatorConventions