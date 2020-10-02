---
title: Best practices
summary: An assortment of best practices presented as DO, DO NOT, and CONSIDER.
reviewed: 2020-10-01
---

This article will present recommendations to keep in mind when designing a system using NServiceBus.

## Architecture

:heavy_check_mark: **DO split up endpoints before scaling out**

Multiple message handlers can be combined inside a single logical endpoint. These handlers all share a single queue for different types of messages. This can decrease throughput of messages.

Instead of scaling out endpoints over different hosts (virtual machines, docker containers, etc), it's better to split up handlers over different logical endpoints. The comparison can also be made to a monolith, which is harder to maintain, update and deploy than smaller services/endpoints. Every logical endpoint can adhere to the Single Responsibility Principle and be responsible for certain types of message handlers. Like `Finance.Invoicing` and `Finance.Orders` and `Finance.Pricing`.

:heavy_check_mark: **DO split up endpoints to separate message processing**

Different types of messages might require a different service-level agreement (SLA). Combining their message handlers inside a single logical endpoint makes it harder to meet a higher SLA, as different message types are waiting to be processed in the same queue.

An example of this are how strategic customers are processed over regular customers. Having different logical endpoints provide options for different SLA and even different business processing of those message. Different ways of processing these message can be read in the blogpost on [why you don't need priority queues](https://bloggingabout.net/2020/07/16/priority-queues-why-you-dont-need-them/).

:x: **DO NOT abstract away NServiceBus**

NServiceBus is itself an abstraction over different message queue technologies, allowing message handlers to concern themselves with business code and not worry about the speific implementation of the underlying queue.

As a dependency, conventional wisdom holds that the NServiceBus API should itself be abstracted behind another abstraction, perhaps as a hedge against a possible future where the system will have to completely switch to a different service bus library.

However, NServiceBus is a fairly opinionated framework that attempts to guide developers building distributed systems into a [pit of success](https://blog.codinghorror.com/falling-into-the-pit-of-success/). It also becomes the communication foundation for the entire system.

Because of these qualities, abstractions over NServiceBus tend to basically mimic the NServiceBus API which add no value. When it comes time to switch to a different service bus framework (which is a dreadfully uncommon activity to start with as it does not deliver any business value) it ends up being much more difficult than simply swapping one implementation for a new one.

Meanwhile, a custom abstraction makes this NServiceBus documentation fairly useless as developers must code to a custom, often undocumented API instead of directly against the NServiceBus API.

:x: **DO NOT use NServiceBus to query data**

When high throughput systems use the traditional approach of persisting data, those systems often run into all kinds of issues due to the nature of how databases work with transactions and locking data. Messaging can eliviate many of these issues as adding messages to a queue doesn't require locking other data. Another benefit is being able to manage the concurrency of a messaging endpoint up to a point that the database can persist data, but more importantly serve data to clients.

These benefits are not applicable when simply retrieving data to provide to clients.

:x: **DO NOT create a messaging endpoint for every single web request**

NServiceBus does a lot of work when it first starts up, scanning through assemblies to find the types for messages and message handlers, establishing communication with the message infrastructure, and making sure that everything is optimized to run quickly for the duration of the endpoint's life. Do not repeat all this work on every web request just to send a single message and then shut down.

An NServiceBus endpoint is designed to be a long-lived object that persists for the entire life of the application process. Once the `IMessageSession` is created, use dependency injection to inject it into controllers or wherever else it is needed. If necessary, assign the `IMessageSession` to a global variable.


:x: **DO NOT use messaging for [data distribution](/nservicebus/messaging/data-distribution.md)**

A common example of a data distribution scenario is having cached data on multiple scaled-out web servers and attempting to deliver a message to each of them. The message indicates each server should drop their current cache entries and retrieve fresh data from the database.

Asynchronous messaging (e.g. NServiceBus) is **not** a good solution for data distribution scenarios. Other technologies that are designed specifically to solve these problems should be used instead. Some examples can be found in the [recommendations section](/nservicebus/messaging/data-distribution.md#recommendations) of the data distribution article.


## Message endpoints

:heavy_check_mark: **DO keep message handlers focused on business code**

Message handlers should be fairly simple, and focused on only the business code needed to handle the message. Infrastructure code for logging, exception management, timing, auditing, authorization, unit of work, message signing/encryption (the list could go on forever) should not be included in a message handler.

Instead, implement this functionality separately in a [message pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md), which enables inserting additional functionality into the NServiceBus message processing pipeline, kind of like an [ASP.NET ActionFilter except for messaging.

For a high-level overview of infrastructure concerns and behaviors, see the blog post [Infrastructure soup](https://particular.net/blog/infrastructure-soup).

:x: **DO NOT catch exceptions in a message handler**

NServiceBus relies on a message handler completing _without_ throwing an exception to know that the message was processed successfully. If an exception is thrown, the message can be retried until it succeeds, which resolves many transient errors. (See the blog post [I caught an exception. Now what?](https://particular.net/blog/but-all-my-errors-are-severe).) If too many retries occur, the message is moved to an error queue where a developer can figure out what's wrong with it and correct the underlying issue.

If exceptions are caught in a message handler, the message will be consumed, without the ability to be retried, regardless of the error.

If it is necessary to catch certain kinds of exceptions, consider throwing a new exception with a more specific message to give context about what is going on in the message handler that led to the failure. Always be sure to include the original error in the constructor of the new exception, so that the `InnerException` property will show the stack trace of the original failure.


:x: **DO NOT [enable installers](/nservicebus/operations/installers.md) in production**

In a development environment, installers create dependencies that the endpoint needs to run, such as queues in the message transport and tables or schema in the persistence store. This is meant to make the development process fast and friction-free because changes are occurring at a rapid pace.

In production scenarios, different permissions are needed to administer resources such as database tables and queues, and the endpoint should not run in production with permissions to alter these resources.

In production, the creation of queues, database tables, and other tables should be scripted as a part of a DevOps process when the endpoint is installed. Methods to administer these resources differ for every message transport and persistence package. For many packages, Particular will provide a `dotnet` global tool, PowerShell library, or other means of performing operational scripting to create endpoints.


:heavy_check_mark: **DO use the [Outbox feature](/nservicebus/outbox/) to provide consistency between business data modifications and messaging operations**

Because message queues generally do not support any form of transactions, message handlers that modify business data in a database run into problems when the messages that are sent become inconsistent with the changes made to business data, resulting in ghost messages or phantom records.

Using the Outbox guarantees exactly-once message processing by piggybacking on the local database transaction used to store business data.

When using transports that only support [receive-only or sends-atomic-with-receives transactions](/transports/transactions.md), the Outbox ensures that outgoing messages are consistent with changes made to business data in the database.


:x: **DO NOT use messaging for occasionally-connected clients**

Message queues are long-lasting and durable. Occasionally-connected clients, such as mobile devices or client-side graphical user interfaces, should not receive messages from message queues. The durable nature of message queuing can result in a backlog of messages for a disconnected client which, if disconnected long enough, can result in queue quotas being exceeded. This can ultimately impact performance in other parts of the system.

For occasionally-connected clients, consider another communication medium, such as in the [Near real-time transient clients sample](/samples/near-realtime-clients/) which communicates with clients using [SignalR](https://dotnet.microsoft.com/apps/aspnet/signalr).


## System monitoring

:heavy_check_mark: **DO install the [Heartbeats plugin](/monitoring/heartbeats/) in all endpoints to monitor for endpoint health**

The Heartbeats plugin sends a message to [ServiceControl](/servicecontrol/) at regular intervals to demonstrate that the process is not only executing (which would be provided by any suite of system monitoring tools) but is also capable of interacting with the message transport.

If ServiceControl stops receiving heartbeat messages from an endpoint, that endpoint will be shown as [inactive in ServicePulse](/monitoring/heartbeats/in-servicepulse.md). In addition, ServiceControl will publish a [`H`eartbeatStopped` event](/monitoring/heartbeats/notification-events.md) so that operations staff can be notified and respond.


:heavy_check_mark: **DO use monitoring**

It's important to know how a system is performing. With the performance monitoring capabilities in ServiceControl and ServicePulse, the queue length, throughput, retry rate, processing time, and critical time for every endpoint can be monitored in a single view with graphs.

Critical time is especially important, as it describes how long it takes for a message to wait in the queue and be processed. Because most processes will (either explicitly or implicitly) come with some sort of service-level agreement (SLA) for how long it can take for a new message of a given type to be processed, monitoring the endpoint's critical time is key to making sure that those SLAs are upheld.

For more information, see an [introductory video on monitoring](https://particular.net/real-time-monitoring), the [getting started guide](/tutorials/monitoring-setup/), or give the self-contained [monitoring demo](/tutorials/monitoring-demo/) a spin.


:heavy_check_mark: **DO create a subscriber for [ServiceControl events](/servicecontrol/contracts.md) to be notified of failed messages and missed heartbeats**

[ServicePulse](/servicepulse/) will display the current status of a system, including failed messages, missed heartbeats, and failed custom checks. However, ServicePulse acts as a dashboard and does not provide any notification when things go wrong.

When these things occur, ServiceControl will publish an NServiceBus message that can be subscribed to like any other NServiceBus message. From this message handler, any action that can be performed from code is possible: send an email to system administrators, notify a Slack channel, log it to a database…anything is possible.

It is better to be notified when things go wrong than to find out after it's too late.


:heavy_check_mark: **CONSIDER [identifying message types using conventions](/nservicebus/messaging/unobtrusive-mode.md) to make upgrading to new versions of NServiceBus easier**

By default, NServiceBus will identify classes implementing `ICommand` as commands, `IEvent` as events, and `IMessage` as other types of messages such as replies. This is quick and easy, but also causes message projects to have a dependency on the NServiceBus NuGet package.

Because NServiceBus is wire-compatible between major versions, in a complex system it's useful to be able to upgrade one endpoint at a time. But message assemblies are shared between multiple endpoints, which can cause challenges during upgrades when one endpoint using a message assembly has upgraded to the next major version, but the other has not.

These versioning problems can be fixed using [assembly binding redirects](https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/redirect-assembly-versions), but it can be easier to use [unobtrusive-mode messages](/nservicebus/messaging/unobtrusive-mode.md) by defining [message conventions](/nservicebus/messaging/conventions.md) independent of the `ICommand`/`IEvent`/`IMessage` interfaces.

These conventions can even be [encapsulated in a class](/nservicebus/messaging/conventions.md#encapsulated-conventions), and many can be used within one endpoint, so that messages from multiple teams who have made different choices on message conventions can be used together.


:x: **AVOID using asynchronous messages for synchronous communication**

It is best to embrace the asynchronous nature of NServiceBus messages, and not use an asynchronous message (or a pair of messages in a request/reply scenario) for synchronous communication, especially when the scenario expects an answer to be available _right now_. This is especially important with queries: [messaging should not be used for queries](http://andreasohlund.net/2010/04/22/messaging-shouldnt-be-used-for-queries/).

When a previously-defined user interface demands an immediate response, such as inserting a new item into a grid and then immediately refreshing the grid to include the new item, the [client-side callbacks package](/nservicebus/messaging/callbacks.md) can be used, but this should be considered a crutch until a more [task- or command-focused UI](https://cqrs.wordpress.com/documents/task-based-ui/) can replace it.
