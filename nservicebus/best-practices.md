---
title: Best practices
summary: An assortment of best practices presented as DO, DO NOT, and CONSIDER.
reviewed: 2020-10-01
---

This article presents recommendations to keep in mind when designing a system using NServiceBus.

## Architecture

:heavy_check_mark: **DO limit the number of handlers in each endpoint**

Multiple message handlers can be combined inside a single logical endpoint. However, these handlers all share a single message queue for different types of messages.

The endpoint is the fundamental unit of scale for an NServiceBus system. If one message handler has much higher throughput requirements, it can only be independently scaled if it exists in an endpoint by itself. In separate endpoints, only the message handler that has the unique scalability requirements has to be scaled out.

The endpoint is also the fundamental unit of deployment for an NServcieBus system. That means that if a fix is required to one message handler, the entire endpoint must be redeployed. The fewer message handlers in each endpoint, the less likely any individual deployment is to cause a problem in the system, since the whole system does not have to be redeployed on every change.


:heavy_check_mark: **CONSIDER grouping message handlers by SLA**

Different message handlers commonly have very different timing requirements. A back-end, largely asynchronous process may take 30 seconds or more to complete, where nobody will notice or care if it happens to take longer. On the other hand, commands sent directly from the customer UI tend to be processed very quickly, and there is an expectation that processing should complete within a second or two.

This expected time is the service-level agreement (SLA) for that message handler. It is unwise to group message handlers with different SLAs in the same endpoint because all the handlers share the same queue. When SLAs are mixed, it becomes possible for a 1-second-SLA message to get stuck in line behind a batch of 60-second-SLA messages, causing the SLA of the shorter message to be breached.

With message handlers separated by SLA, the scalability needs for the message handlers can be adjusted to make sure that the [critical time](/monitoring/metrics/definitions.md#metrics-captured-critical-time) (the amount of time the message waits to be processed + the processing time) does not exceed that SLA.

For more information on monitoring the critical time, see the [NServiceBus monitoring setup guide](/tutorials/monitoring-setup/).


:x: **DO NOT abstract away NServiceBus**

NServiceBus is itself an abstraction over different message queue technologies, allowing message handlers to concern themselves with business code and not worry about the speific implementation of the underlying queue.

As a dependency, conventional wisdom holds that the NServiceBus API should itself be abstracted behind another abstraction, perhaps as a hedge against a possible future where the system will have to switch to a different service bus library.

However, NServiceBus is an opinionated framework that attempts to guide developers building distributed systems into a [pit of success](https://blog.codinghorror.com/falling-into-the-pit-of-success/). It also becomes the communication foundation for the entire system.

Because of these qualities, abstractions over NServiceBus tend to mimic the NServiceBus API which adds no value. When it comes time to switch to a different service bus framework (which is an uncommon activity to start with as it does not deliver any business value) it ends up being much more difficult than simply swapping one implementation for a new one.

Meanwhile, a custom abstraction makes the NServiceBus documentation less effective as developers must code to a custom, often undocumented API instead of directly against the NServiceBus API.


:x: **AVOID using asynchronous messages for synchronous communication, including queries**

It is best to embrace the asynchronous nature of NServiceBus messages, and not use an asynchronous message (or a pair of messages in a request/reply scenario) for synchronous communication, especially when the scenario expects an answer to be available _right now_. This is especially important with queries: [messaging should not be used for queries](http://andreasohlund.net/2010/04/22/messaging-shouldnt-be-used-for-queries/).

When a previously-defined user interface demands an immediate response, such as inserting a new item into a grid and then immediately refreshing the grid to include the new item, the [client-side callbacks package](/nservicebus/messaging/callbacks.md) can be used, but this should be considered a crutch until a more [task- or command-focused UI](https://cqrs.wordpress.com/documents/task-based-ui/) can replace it.


:x: **DO NOT create a messaging endpoint for every single web request**

NServiceBus does a lot of work when it first starts up, scanning through assemblies to find the types for messages and message handlers, establishing communication with the message infrastructure, and making sure that everything is optimized to run quickly for the duration of the endpoint's life. Do not repeat all this work on every web request just to send a single message and then shut down.

An NServiceBus endpoint is designed to be a long-lived object that persists for the entire life of the application process. Once the `IMessageSession` is created, use dependency injection to inject it into controllers or wherever else it is needed. If necessary, assign the `IMessageSession` to a global variable.


:x: **DO NOT use messaging for [data distribution](/nservicebus/concepts/data-distribution.md)**

A common example of a data distribution scenario is having cached data on multiple scaled-out web servers and attempting to deliver a message to each of them. The message indicates each server should drop their current cache entries and retrieve fresh data from the database.

Asynchronous messaging (e.g. NServiceBus) is **not** a good solution for data distribution scenarios. Other technologies that are designed specifically to solve these problems should be used instead. Some examples can be found in the [recommendations section](/nservicebus/concepts/data-distribution.md#recommendations) of the data distribution article.


## Message endpoints

:heavy_check_mark: **DO keep message handlers focused on business code**

Message handlers should be simple, and focused on only the business code needed to handle the message. Infrastructure code for logging, exception management, timing, auditing, authorization, unit of work, message signing/encryption, etc, should not be included in a message handler.

Instead, implement this functionality separately in a [message pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md), which enables inserting additional functionality into the NServiceBus message processing pipeline, similar to an ASP.NET ActionFilter.

For a high-level overview of infrastructure concerns and behaviors, see the blog post [Infrastructure soup](https://particular.net/blog/infrastructure-soup).

:x: **DO NOT catch exceptions in a message handler**

NServiceBus relies on a message handler completing _without_ throwing an exception to know that the message was processed successfully. If an exception is thrown, the message can be retried until it succeeds, which resolves many transient errors. (See the blog post [I caught an exception. Now what?](https://particular.net/blog/but-all-my-errors-are-severe).) If too many retries occur, the message is moved to an error queue where a developer can use [ServicePulse](/servicepulse/) to [view the failed message details](/servicepulse/intro-failed-messages.md) to figure out what's wrong with it and correct the underlying issue.

If the message handler catches the exception, the message will be consumed and removed from the queue, without the ability to be retried, regardless of the error.

If it is necessary to catch certain kinds of exceptions, consider throwing a new exception with a more specific message to give context about what is going on in the message handler that led to the failure. Always be sure to include the original error in the constructor of the new exception, so that the `InnerException` property will show the stack trace of the original failure.


:x: **DO NOT [enable installers](/nservicebus/operations/installers.md) in production**

In a development environment, installers create dependencies that the endpoint needs to run, such as queues in the message transport and tables or schema in the persistence store. This is meant to make the development process fast and friction-free because changes are occurring at a rapid pace.

In production scenarios, different permissions are needed to administer resources such as database tables and queues, and the endpoint should not run in production with permissions to alter these resources.

In production, the creation of queues, database tables, and other tables should be scripted as a part of a DevOps process when the endpoint is installed. Methods to administer these resources differ for every message transport and persistence package. For many packages, Particular will provide a `dotnet` global tool, PowerShell library, or other means of performing operational scripting to create endpoints.


:heavy_check_mark: **DO use the [Outbox feature](/nservicebus/outbox/) to provide consistency between business data modifications and messaging operations**

Because message queues generally do not support any form of transactions, message handlers that modify business data in a database run into problems when the messages that are sent become inconsistent with the changes made to business data, resulting in ghost messages or phantom records.

Using the outbox guarantees exactly-once message processing by taking advantage of the local database transaction used to store business data.

When using transports that only support [receive-only or sends-atomic-with-receives transactions](/transports/transactions.md), the outbox ensures that outgoing messages are consistent with changes made to business data in the database.


:x: **DO NOT use messaging for occasionally-connected clients**

Message queues are long-lasting and durable. Occasionally-connected clients, such as mobile devices or client-side graphical user interfaces, should not receive messages from message queues. The durable nature of message queuing can result in a backlog of messages for a disconnected client which, if disconnected long enough, can result in queue quotas being exceeded. This can ultimately impact performance in other parts of the system.

For occasionally-connected clients, consider another communication medium, such as in the [Near real-time transient clients sample](/samples/near-realtime-clients/) which communicates with clients using [SignalR](https://dotnet.microsoft.com/apps/aspnet/signalr).


:heavy_check_mark: **CONSIDER [identifying message types using conventions](/nservicebus/messaging/unobtrusive-mode.md) to make upgrading to new versions of NServiceBus easier**

By default, NServiceBus will identify classes implementing `ICommand` as commands, `IEvent` as events, and `IMessage` as other types of messages such as replies. This is quick and easy, but also causes message projects to have a dependency on the NServiceBus NuGet package.

Because NServiceBus is wire-compatible between major versions, in a complex system it's useful to be able to upgrade one endpoint at a time. But message assemblies are shared between multiple endpoints, which can cause challenges during upgrades when one endpoint using a message assembly has upgraded to the next major version, but the other has not.

These versioning problems can be addressed using [unobtrusive-mode messages](/nservicebus/messaging/unobtrusive-mode.md) by defining [message conventions](/nservicebus/messaging/conventions.md) independent of the `ICommand`/`IEvent`/`IMessage` interfaces.

These conventions can even be [encapsulated in a class](/nservicebus/messaging/conventions.md#encapsulated-conventions), and many can be used within one endpoint, so that messages from multiple teams who have made different choices on message conventions can be used together.


## System monitoring

:heavy_check_mark: **DO install the [Heartbeats plugin](/monitoring/heartbeats/) in all endpoints to monitor for endpoint health**

The Heartbeats plugin sends a message to [ServiceControl](/servicecontrol/) at regular intervals to demonstrate that the process is not only executing (which would be provided by any suite of system monitoring tools) but is also capable of interacting with the message transport.

If ServiceControl stops receiving heartbeat messages from an endpoint, that endpoint will be shown as [inactive in ServicePulse](/monitoring/heartbeats/in-servicepulse.md). In addition, ServiceControl will publish a [`HeartbeatStopped` event](/monitoring/heartbeats/notification-events.md) so that operations staff can be notified and respond.


:heavy_check_mark: **DO monitor distributed systems**

It's important to know how a system is performing. With the performance monitoring capabilities in ServiceControl and ServicePulse, the queue length, throughput, retry rate, processing time, and critical time for every endpoint can be monitored in a single view with graphs.

Critical time is especially important, as it describes how long it takes for a message to wait in the queue and be processed. Because most processes will (either explicitly or implicitly) come with some form of service-level agreement (SLA) for how long it can take for a new message of a given type to be processed, monitoring the endpoint's critical time is key to making sure that those SLAs are upheld.

For more information, see an [introductory video on monitoring](https://particular.net/real-time-monitoring), the [monitoring setup guide](/tutorials/monitoring-setup/), or the self-contained [monitoring demo](/tutorials/monitoring-demo/).


:heavy_check_mark: **DO create a subscriber for [ServiceControl events](/servicecontrol/contracts.md) to be notified of failed messages and missed heartbeats**

[ServicePulse](/servicepulse/) will display the current status of a system, including failed messages, missed heartbeats, and failed custom checks. However, ServicePulse acts as a dashboard and does not provide any notification when things go wrong.

When these things occur, ServiceControl will publish an NServiceBus message that can be subscribed to like any other NServiceBus message. From this message handler, any action that can be performed from code is possible: send an email to system administrators, notify a Slack channel, log it to a database...anything is possible.

It is better to be notified as soon as something has gone wrong than to find out after it's too late.
