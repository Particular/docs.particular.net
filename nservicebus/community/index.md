---
title: Community extensions and integrations
summary: curated list of community-developed extensions and integrations for the NServiceBus ecosystem.
reviewed: 2020-10-12
redirects:
 - nservicebus/mailer
 - samples/mailer
 - nservicebus/dependency-injection/simpleinjector
 - samlpes/dependency-injection/simpleinjector
 - transports/eventstore
 - nservicebus/gateway/httpvnext-channel
 - nservicebus/audit-filter
 - nservicebus/handlers/handler-ordering-by-interface
 - nservicebus/messaging/attachments-fileshare
 - nservicebus/messaging/attachments-sql
 - nservicebus/messaging/validation-dataannotations
 - nservicebus/messaging/validation-fluentvalidation
 - nservicebus/serialization/bond
 - nservicebus/serialization/hyperion
 - nservicebus/serialization/jil
 - nservicebus/serialization/messagepack
 - nservicebus/upgrades/messagepack-changes
 - nservicebus/serialization/msgpack
 - nservicebus/serialization/protobufgoogle
 - nservicebus/serialization/protobufnet
 - nservicebus/serialization/utf8json
 - nservicebus/serialization/zeroformatter
 - samples/attachments-fileshare
 - samples/attachments-sql
 - samples/audit-filter
 - samples/data-annotations-validation
 - samples/encryption/newtonsoft-json-encryption
 - samples/fluent-validation
 - samples/handler-ordering-by-interface
 - samples/serializers/bond
 - samples/serializers/hyperion
 - samples/serializers/jil
 - samples/serializers/messagepack
 - samples/serializers/msgpack
 - samples/serializers/protobufgoogle
 - samples/serializers/protobufnet
 - samples/serializers/utf8json
 - samples/serializers/zeroformatter
 - samples/sqltransport/sql-native
 - samples/web/sql-http-passthrough
 - transports/sql/sql-http-passthrough
 - transports/sql/sql-native
---

This is a curated list of community-developed extensions and integrations for the NServiceBus ecosystem.

Community projects are maintained by community members and are **not covered** by the [Particular Software License](https://particular.net/licensing) or the [Particular Software Support Agreement](https://particular.net/supportagreement). Each project is covered by its own license and terms. It's possible that a community contribution may not be updated to support the most recent version of NServiceBus.


## [Aggregates.NET](https://github.com/charlessolar/Aggregates.NET)

A framework to help developers integrate NServiceBus with [EventStore](https://github.com/EventStore/EventStore).


## [Community.NServiceBus.LambdaHandlers](https://github.com/timbussmann/Community.NServiceBus.LambdaHandlers)

Supports declarative delegate-based message handlers for NServiceBus.

Created by [Tim Bussmann](https://github.com/timbussmann).


## [NServiceBus.Gateway.Channels.HttpVNext](https://github.com/welshdave/NServiceBus.Gateway.Channels.HttpVNext)

An HTTP channel implementation for the [NServiceBus Gateway](/nservicebus/gateway/) that doesn't use HTTP headers for message content or metadata. This makes it easier to use this channel in situations where HTTP headers may be modified, such as when a gateway is behind a reverse proxy such as NGINX.

Created by [Dave Lewis](https://www.dllewis.org/).


## [NServiceBus.MessageRouting](https://github.com/jbogard/NServiceBus.MessageRouting)

Provides an implementation of the [routing slip pattern](http://www.enterpriseintegrationpatterns.com/RoutingTable.html) in NServiceBus message handlers to allow a predefined workflow without the need for a saga.

Created by [Jimmy Bogard](https://jimmybogard.com/).


## [NServiceBus.Mailer](https://github.com/HEskandari/NServiceBus.Mailer)

Provides a method to more easily and reliably send emails from an NServiceBus message handler.

Created by [Hadi Eskandari](http://www.seesharpsoftware.com.au/).


## [NServiceBus.AttributeConventions](https://github.com/mauroservienti/NServiceBus.AttributeConventions)

Enables identifying NServiceBus messages, commands, and events by conventions using attributes i.e. `[Command]` and `[Event]` instead of the built-in `ICommand` and `IEvent` marker interfaces.

Created by [Mauro Servienti](https://milestone.topics.it/).


## [Rabbit Operations](http://rabbitoperations.southsidesoft.com/)

Provides operations support for RabbitMQ applications that run on NServiceBus via a graphical user interface.

Created by [SouthsideSoft](http://southsidesoft.com/).


## [Verify.NServiceBus](https://github.com/NServiceBusExtensions/Verify.NServiceBus)

Adds [Verify](https://github.com/VerifyTests/Verify) support to verify NServiceBus test contexts. Given an NServiceBus message handler, Verify.NServiceBus writes the results of the handler's execution (messages sent, published, etc.) to a file that is diffed against the previous test run to make sure the results are as expected without needing to write multiple assertions for each property.

Part of the [NServiceBusExtensions](https://github.com/NServiceBusExtensions) suite of extensions.


## [NServiceBus.Attachments](https://github.com/NServiceBusExtensions/NServiceBus.Attachments)

Adds a streaming based attachment functionality to NServiceBus.

Part of the [NServiceBusExtensions](https://github.com/NServiceBusExtensions) suite of extensions.


## [NServiceBus.AuditFilter](https://github.com/NServiceBusExtensions/NServiceBus.AuditFilter)

Adds audit message filtering functionality to NServiceBus, so that certain message types can be included or excluded from normal NServiceBus auditing by adding an attribute to the message type.

Part of the [NServiceBusExtensions](https://github.com/NServiceBusExtensions) suite of extensions.


[Newtonsoft.Json.Encryption](https://github.com/NServiceBusExtensions/Newtonsoft.Json.Encryption)

Leverages the Newtonsoft extension API to encrypt/decrypt specific nodes at serialization time. So only the nodes that require encryption are touched, the remaining content is still human readable. This approach provides a compromise between readability/debugability and security.

Part of the [NServiceBusExtensions](https://github.com/NServiceBusExtensions) suite of extensions.


## [NServiceBus.SqlServer.Native](https://github.com/NServiceBusExtensions/NServiceBus.Native)

A shim providing low-level access to the [NServiceBus SQL Server Transport](/transports/sql/) with no NServiceBus or SQL Server Transport reference required.

Part of the [NServiceBusExtensions](https://github.com/NServiceBusExtensions) suite of extensions.


## [NServiceBus Validation](https://github.com/NServiceBusExtensions/NServiceBus.Validation)

Allows validating message contents with options to use [DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations) and [FluentValidation](https://github.com/JeremySkinner/FluentValidation).

Part of the [NServiceBusExtensions](https://github.com/NServiceBusExtensions) suite of extensions.


## Serializers

These packages add support for alternate message serialization technologies. These can be useful in situations where special requirements for messages serialization, such as speed, compactness, or integration with external systems are necessary.

### NServiceBusExtensions Serializers

These serializer packages are part of the [NServiceBusExtensions](https://github.com/NServiceBusExtensions) suite of extensions:

* [Bond](https://github.com/NServiceBusExtensions/NServiceBus.Bond)
* [Hyperion](https://github.com/NServiceBusExtensions/NServiceBus.Hyperion)
* [Jil](https://github.com/NServiceBusExtensions/NServiceBus.Jil)
* [MessagePack](https://github.com/NServiceBusExtensions/NServiceBus.MessagePack)
* [MsgPack](https://github.com/NServiceBusExtensions/NServiceBus.MsgPack)
* [ProtoBuf-Google](https://github.com/NServiceBusExtensions/NServiceBus.ProtoBufGoogle)
* [ProtoBuf-Net](https://github.com/NServiceBusExtensions/NServiceBus.ProtoBufNet)
* [Utf8Json](https://github.com/NServiceBusExtensions/NServiceBus.Utf8Json)
* [ZeroFormatter](https://github.com/NServiceBusExtensions/NServiceBus.ZeroFormatter)
