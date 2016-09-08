---
title: Upgrade Version 5 to 6
summary: Instructions on how to upgrade NServiceBus Version 5 to 6.
tags:
 - upgrade
 - migration
related:
- nservicebus/upgrades/gateway-1to2
- nservicebus/upgrades/sqlserver-2to3
---


## Upgrading to NServiceBus 6

Every solution is different and will encounter unique challenges when upgrading a major dependency like NServiceBus. It's important to plan out an upgrade project and proceed in well defined steps, stopping to ensure that everything is working after each step. Here are a few things to consider when planning an upgrade project.

### Endpoint selection

It is not necessary for every endpoint in the solution to be running the same version of NServiceBus. Endpoints running Version 6 are able to exchange messages with endpoints running Version 5 transparently. This [wire compatability](link to wire compat doco) helps to reduce the complexity of an upgrade project by allowing each endpoint to be upgraded one at a time. 

Not every endpoint in the solution needs to be upgraded to Version 6 at all. Each endpoint only needs to be upgraded if it will take advantage of a new feature introduced in Version 6. New endpoints added to the system can be started, developed and deployed entirely in Version 6 and will be able to exchange messages with the other endpoints in the solution that are on Version 5.

**Do not upgrade an endpoint unless there is a compelling reason to do so.**

Note that some new features added in Version 6 require that all endpoints are running on Version 6 before they can be switched on (can we identify even a subset of these?). Another factor to consider is the investment required to maintain codebases using different versions of NServiceBus. It may be cheaper in the long run to maintain a single codebase containing just Version 6 code than to invest in training and knowledge around Versions 5 and below.

Once the list of endpoints that need to be upgraded to Version 6 has been identified, upgrade them one at a time. As a Version 6 endpoint is able to exchange messages with Version 5 endpoints, upgrade one endpoint, test it, and deploy it to production before upgrading the next endpoint. This keeps the scope of changes to a minimum which help to reduce risk and to isolate potential problems when they arise. 

**Upgrade one endpoint at a time.**

There is one common issue with upgrading a single endpoint at a time. If the endpoints in a solution share a common libary then upgrading one endpoint might lead to changes in the common library and that necessitates changes in all of the other endpoints that rely on the common library at the same time. The recommended approach to dealing with this is to create a copy of the common libary for the new endpoint and to upgrade it along with the endpoint. When the time comes to upgrade the second endpoint, change it's dependency to point to the new, upgraded, version of the common library. When using this approach, other changes to the common library should be minimized as they will need to be reflected in both codebases.

The process of upgrading each endpoint is going to follow a common sequence of steps. Being able to repeatably apply those steps is key to the success of the upgrade project. The recommended approach is to upgrade a simple and low risk endpoint first to ensure that the process is well understood before tackling the endpoints that make up the core of the solution. Endpoints that send email or generate documents are often good candidates for this. When selecting the first endpoint to upgrade look for a small number of reasonably straightforward handlers and a small amount of NServiceBus configuration. It is worth considering selecting a simple endpoint to upgrade even if it will not take advantage of Version 6 features to practice the upgrade process. 


### Move to .NET 4.5.2

The minimum .NET version for NServiceBus Version 6 is .NET 4.5.2.

**All projects (that reference NServiceBus) must be updated to .NET 4.5.2 before updating to NServiceBus Version 6.**

It is recommended to update to .NET 4.5.2 and perform a full migration to production **before** updating to NServiceBus Version 6.

For larger solutions the Visual Studio extension [Target Framework Migrator](https://visualstudiogallery.msdn.microsoft.com/47bded90-80d8-42af-bc35-4736fdd8cd13) can reduce the manual effort required in performing an upgrade.


### Update NServiceBus packages

`Update-Package NServiceBus`

Check for other dependencies
This will cause a lot of errors
This is to be expected


#### IBus, IStartableBus and the Bus Static class are now obsolete

In previous versions of NServiceBus, to send or publish messages within a message handler or other extension interfaces, the message session (`IBus` interface in Versions 5 and below) was accessed via container injection. In Versions 6 injecting the message session is no longer required. Message handlers and other extension interfaces now provide context parameters such as `IMessageHandlerContext` or `IEndpointInstance` which give access to the same functions that used to be available via the `IBus` interface.

INFO: For more details on the various scenarios when using IBus, see: [Migrating from IBus](moving-away-from-ibus.md).


### Update Endpoint configuration

BusConfig vs EndpointConfig
Bus vs Endpoint
The Host needs to be mentioned here too
Link off to documentation covering the changes to be made when updating config


### Update Handlers

Short explanation
Link off to full doco about how to do this


### Update sending messages

Now that Handler Contexts have been introduced, talk about bus.Send and bus.Publish outside of handlers


### Update Sagas

Should be simple given the above topics.


### Finishing touches

At this point the number of issues remaining should be minimal. Link off to the full upgrade guide for all of the remaining topics. 