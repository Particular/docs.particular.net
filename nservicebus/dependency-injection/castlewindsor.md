---
title: Castle Windsor
summary: Details on how to Configure NServiceBus to use Castle Windsor for dependency injection. Includes usage examples as well as lifecycle mappings.
component: Castle
reviewed: 2016-11-28
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/castle
redirects:
 - nservicebus/containers/castle
---


NServiceBus can be configured to use [Castle Windsor](https://github.com/castleproject/Windsor) for dependency injection.


### Default Usage

snippet: CastleWindsor


### Existing Instance

snippet: CastleWindsor_Existing


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/dependency-injection/#dependency-lifecycle) map to [Castle LifestyleType](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md) in the following way.


| DependencyLifecycle                                                                                             | Castle LifestyleType                                                                           |
|-----------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instancepercall) | [Transient](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#transient) |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instanceperunitofwork)                    | [Scoped](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#scoped)       |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-singleinstance)                                  | [Singleton](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#singleton) |
