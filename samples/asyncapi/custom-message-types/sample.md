---
title: AsyncAPI with custom message conventions
summary: How to generate an AsyncAPI schema document from NServiceBus with custom message conventions
reviewed: 2025-08-13
component: Core
related:
- samples/asyncapi/simple
---

This sample demonstrates how to generate an [AsyncAPI](https://www.asyncapi.com/en) schema document from NServiceBus endpoints using [Neuroglia.AsyncApi](https://github.com/asyncapi/net-sdk) in two different hosting environments:

- [.NET Generic Host](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host)
- [ASP.NET Core Web Host](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host)

It extends the [simple AsyncAPI sample](/samples/asyncapi/simple) by differentiating between event types (i.e. published vs subscribed) and showing how an event can be published by one endpoint and subscribed to by another without using the same concrete class to define the event, therefore decoupling the systems from each other.

> [!NOTE]
> AsyncAPI integration is not an officially supported feature of NServiceBus, and this example is for demonstration purposes only.

## Code walk-through

This sample contains four projects:

- [AsyncAPI.Feature](#code-walk-through-feature-project) - a class library containing shared code required to enable the custom `AsyncApiFeature`
- [AsyncAPI.Web](#code-walk-through-webapi-project) - a ASP.NET Core WebAPI application that publishes two events and generates an AsyncAPI document schema for its structure, accessible via a URL
- [AsyncAPI.GenericHost](#code-walk-through-generic-host-project) - a console application that publishes two events, sends a message and generates an AsyncAPI document schema for its structure and saves it to disk
- [Subscriber](#code-walk-through-subscriber-project) - a console application that subscribes to the events published by AsyncAPI.GenericHost and AsyncAPI.Web

### Feature project

The project contains all necessary code to support decoupling event types from the publisher and subscriber (contained in the `MessageConventions` folder), as well as registering a custom AsyncAPI document schema generator.

The `EndpointConfigurationExtensions` contains the glue that registers the `AsyncApiFeature` and the endpoint configuration message conventions.

snippet: EnableAsyncApiSupport

#### AsyncAPI feature

The feature creates mappings for physical to logical event types based on the `PublishedEvent` and `SubscribedEvent` attributes that decorate the events that are published and subscribed to. These mappings are registered in the container so that they are available for the following [pipeline behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md) defined in the feature:

- ReplaceOutgoingEnclosedMessageTypeHeaderBehavior
- ReplaceIncomingEnclosedMessageTypeHeaderBehavior
- ReplaceMulticastRoutingBehavior

snippet: RegisterEventMappings

Finally, the code registers a custom implementation of the Neuroglia `IAsyncApiDocumentGenerator` which will be used instead of the default implementation to generate the NServiceBus-specific schema document.

snippet: RegisterCustomDocumentGenerator

##### AsyncAPI document generator

This custom implementation of the Neuroglia `IAsyncApiDocumentGenerator` creates one AsyncAPI schema document for the NServiceBus endpoint hosted in the application. It demonstrates how channel information with the endpoint's address (queue name) can be generated, containing the publish operation and the event payload.

This code can be extended to include subscriptions as well as sent/received messages.

### WebAPI project

#### Setup AsyncAPI

The project enables AsyncAPI schema generation using two setup calls.

First, by adding the Neuroglia AsyncAPI.

snippet: WebAppAddNeurogliaAsyncApi

Second, by registering the AsyncAPI feature with the NServiceBus endpoint.

snippet: WebAppEnableAsyncAPIOnNSB

#### Define published events

The published events are defined by using the `PublishedEvent` attribute to tell NServiceBus which event they represent.

The `FirstEventThatIsBeingPublished` class is marked as representing the `SomeNamespace.FirstEvent` event.

snippet: WebAppPublisherFirstEvent

The `SecondEventThatIsBeingPublished` class is marked as representing the `SomeNamespace.SecondEvent` event.

snippet: WebAppPublisherSecondEvent

#### Access AsyncAPI schema document

The resulting AsyncAPI schema document can be accessed under [/asyncapi](https://localhost:7198/asyncapi). From here it can be downloaded and inspected using [AsyncAPI Studio](https://studio.asyncapi.com/) (by pasting in the contents) and incorporated into internal system documentation.

### Generic host project

#### Setup AsyncAPI

The project enables AsyncAPI schema generation using three setup calls.

First, by adding the Neuroglia AsyncAPI.

snippet: GenericHostAddNeurogliaAsyncApi

Second, by registering the AsyncAPI feature with the NServiceBus endpoint.

snippet: GenericHostEnableAsyncAPIOnNSB

Lastly, by adding a background service to generate and write the AsyncAPI document schema to disk.

snippet: GenericHostAddSchemaWriter

#### Define published events

The published events are defined by using the `PublishedEvent` attribute to tell NServiceBus which event they represent.

The `FirstEventThatIsBeingPublished` class is marked as representing the `SomeNamespace.FirstEvent` event.

snippet: GenericHostPublisherFirstEvent

The `SecondEventThatIsBeingPublished` class is marked as representing the `SomeNamespace.SecondEvent` event.

snippet: GenericHostPublisherSecondEvent

One message (`MessageBeingSent`) is also sent and received locally, demonstrating that standard NServiceBus message processing can run alongside custom Publish/Subscribe translations.

#### Access AsyncAPI schema document

The `AsyncAPISchemaWriter` uses the custom document generator injected as part of the `AsyncApiFeature` to generate the document schema and write it to disk.

snippet: GenericHostCustomGenerationAndWritingToDisk

The file is saved into the default `Downloads` folder - it can be viewed using [AsyncAPI Studio](https://studio.asyncapi.com/) (by pasting in the contents) and incorporated into internal system documentation.

### Subscriber project

#### Setup AsyncAPI

The project uses the `EnableAsyncApiSupport` extension method to allow it to subscribe to published events from the [WebAPI](#code-walk-through-webapi-project) and [Generic host](#code-walk-through-generic-host-project) projects using its own implementation of the event classes.

snippet: SubscriberEnableAsyncAPIOnNSB

#### Define subscribed to events

It defines its own concrete event classes and uses the `SubscribedEvent` attribute to tell NServiceBus which event they represent.

The `FirstSubscribedToEvent` class is marked as representing the `SomeNamespace.FirstEvent` event.

snippet: SubscriberFirstEvent

The `SecondSubscribedToEvent` class is marked as representing the `SomeNamespace.SecondEvent` event.

snippet: SubscriberSecondEvent

Note that the defined events do not need to match all the properties defined in the published events, but contain only those that they are interested in.

## Running the sample

When running the solution with Visual Studio, three applications will start automatically.

### AsyncAPI.GenericHost (console)

- Press `s` to send a message to itself.
- Press `1` to publish `FirstEventThatIsBeingPublished` - received by `AsyncAPI.Subscriber`.
- Press `2` to publish `SecondEventThatIsBeingPublished` - received by `AsyncAPI.Subscriber`.

### AsyncAPI.Web (web)

- Open [/scalar](https://localhost:7198/scalar) to publish both events - received by `AsyncAPI.Subscriber`.
- Open [/asyncapi](https://localhost:7198/asyncapi) to view the AsyncAPI schema for the application. This can be exported as JSON or YAML.

### AsyncAPI.Subscriber (console)

Displays all events published from `AsyncAPI.GenericHost` and `AsyncAPI.Web`.
