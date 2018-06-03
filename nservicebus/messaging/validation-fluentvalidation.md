---
title: FluentValidation message validation
summary: Validate message using FluentValidation.
reviewed: 2018-06-02
component: FluentValidation
related:
---

Uses [FluentValidation](https://github.com/JeremySkinner/FluentValidation) to validate incoming and outgoing messages.

FluentValidation message validation can be enabled using the following:

snippet: FluentValidation

include: validationexception

By default only incoming messages are validated. To also validate outgoing messages use the following:

snippet: FluentValidation_outgoing

include: validationoutgoing

Messages can then have an associated [validator](https://github.com/JeremySkinner/FluentValidation/wiki/b.-Creating-a-Validator):

snippet: FluentValidation_message

## Validator scanning

Validators are registered and resolved using [dependency injection](/nservicebus/dependency-injection/). Assemblies can be added for validator scanning using either a generic Type, a Type instance, or an assembly instance.

snippet: FluentValidation_AddValidators

Validator lifecycle can either be per endpoint ([Single instance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)):

snippet: FluentValidation_EndpointLifecycle

Or [Instance per unit of work](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work):

snippet: FluentValidation_UnitOfWorkLifecycle