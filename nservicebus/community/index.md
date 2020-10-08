---
title: Community extensions and integrations
summary: curated list of community-developed extensions and integrations for the NServiceBus ecosystem.
reviewed: 2020-10-08
redirects:
 - nservicebus/mailer
 - samples/mailer
 - nservicebus/dependency-injection/simpleinjector
 - samlpes/dependency-injection/simpleinjector
 - transports/eventstore
 - nservicebus/gateway/httpvnext-channel
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

Provides operations support for RabbitMQ applications that run on NServiceBus via a grahpical user interface.

Created by [SouthsideSoft](http://southsidesoft.com/).