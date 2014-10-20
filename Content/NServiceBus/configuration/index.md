---
title: Introduction to Configuration API
summary: Introduction to NServiceBus Configuration API
tags:
- NServiceBus
- Fluent Configuration
- BusConfiguration
- V3
- V4
- V5
---

Every NServiceBus endpoint to work properly relies on a configuration to determine settings and behaviors other than endpoint core functionalities. Depending on the used NServiceBus version, V3, V4 or V5, the configuration API and it's availabale features are different.

## NServiceBus V3 and V4

#### Configuration entry point

[Configuration entry point](V3-V4-entry-point) in V3 and V4 is the `Configure` class.

#### Configuration Customization

* [Endpoint Naming](V3-V4-endpoint-naming)
* [Dependency Injection](V3-V4-Dependency-Injection)
* Message identification
	* [Builtin interfaces](V3-V4-Builtin-interfaces)
	* [Unobtrusive Mode](V3-V4-Unobtrusive-Mode)
* [DataBus](V4-Data-Bus)
* [Encryption](V3-V4-Encryption)
* [Logging](V3-V4-Logging)
* [Fault Management](V3-V4-Fault-Management)
* [Performance Counters](V3-V4-Performance-Counters)
* [Service Level Agreement](V3-V4-Service-Level-Agreement)
* Persistence
	* [RavenDB Persistence](V3-V4-RavenDB-Persistence)
	* [NHibernate Persistence](V3-V4-NHibernate-Persistence)
	* [InMemory Persistence](V3-V4-InMemory-Persistence)
* [License](V3-V4-License)
* [Queue Management](V3-V4-Queue-Management)
* [Creating and Starting the Bus](V3-V4-Creating-Starting-Bus)

## NServiceBus V3

#### Configuration Customization

* [MSMQ Transport](V3-MSMQ-Transport)
* [MSMQ Persistence](V3-MSMQ-Persistence)

## NServiceBus V4

#### Configuration Customization

* [Transports](V4-Transports)
* DataBus
	* [Azure and the DataBus](V4-Azure-Data-Bus)

## NServiceBus V5 

#### Configuration entry point

[Configuration entry point](V5-entry-point) in V5 is the `BusConfiguration` class.

#### Configuration Customization

*	Endpoint Naming, Versioning and Address
	* [Naming](V5-endpoint-naming)
	* [Versioning](V5-endpoint-versioning)
	* [Address](V5-endpoint-address)
* [Dependency Injection](V5-dependency-injection)
* [Transport](V5-transport)
* [Serialization](V5-serialization)
* [Transactions](V5-transactions)
* [Outbox](V5-outbox)
* Message identification
	* [Builtin interfaces](V5-Builtin-interfaces)
	* [Unobtrusive Mode](V5-Unobtrusive-Mode)
* [DataBus](V5-Data-Bus)
	* [Azure and the DataBus](V5-Azure-Data-Bus)
* [Encryption](V5-Encryption)
* [Message Handlers Order](V5-Message-Handlers-Order)
* [Subscriptions](V5-Subscriptions)
* [Logging](V5-Logging)
* [Fault Management](V5-Fault-Management)
* [Performance Counters](V5-Performance-Counters)
* [Service Level Agreement](V5-Service-Level-Agreement)
* [Persistence](V5-Persistence)
	* [RavenDB Persistence](V5-RavenDB-Persistence)
	* [NHibernate Persistence](V5-NHibernate-Persistence)
	* [InMemory Persistence](V5-InMemory-Persitence)
* [License](V5-License)
* [Queue Management](V5-Queue-Management)
* [Creating and Starting the Bus](V5-Creating-Starting-Bus)
* [Installers](V5-Installers)
* [Scale out settings for broker scenarios](V5-Scale-out)

### Features

NServiceBus V4 introduce the concept of features. A *feature* is a high level concept that encapsulates a set of settings related to a certain feature. Features can be enabled or disabled. Enabled features can be configured.

### Pipeline

NServiceBus V5 introduce a new message handling pipeline, at configuration time it is possibile to interact with the pipeline configuration exposed by the `Pipeline` property.

An introduction to the Pipeline is available in the [Message Handling Pipeline](/nservicebus/nservicebus-pipeline-intro) article.

### Resources

More about [configuration customization](/nservicebus/customizing-nservicebus-configuration).
Configuration API webminar recording [Mastering NServiceBus Configuration](Mastering NServiceBus Configuration)