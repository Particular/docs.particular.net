---
title: Castle Windsor
summary: Details on how to Configure NServiceBus to use Castle Windsor as a container. Includes usage examples as well as lifecycle mappings.
component: Castle
reviewed: 2016-11-28
tags:
- Dependency Injection
related:
- samples/containers/castle
---


NServiceBus can be configured to use [Castle Windsor](https://github.com/castleproject/Windsor) as a dependency injection container. 


### Default Usage

snippet: CastleWindsor


### Existing Container Instance

snippet: CastleWindsor_Existing


### DependencyLifecycle Mapping

The [DependencyLifecycle](/nservicebus/containers/#dependency-lifecycle) map to [Castle LifestyleType](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md) in the following way.


| DependencyLifecycle                                                                                             | Castle LifestyleType                                                                           |
|-----------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/containers/#dependency-lifecycle-instancepercall) | [Transient](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#transient) |
| [InstancePerUnitOfWork](/nservicebus/containers/#dependency-lifecycle-instanceperunitofwork)                    | [Scoped](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#scoped)       |
| [SingleInstance](/nservicebus/containers/#dependency-lifecycle-singleinstance)                                  | [Singleton](https://github.com/castleproject/Windsor/blob/master/docs/lifestyles.md#singleton) |
