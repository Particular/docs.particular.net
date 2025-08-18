---
title: Simple AsyncAPI
summary: How to generate an AsyncAPI schema document from NServiceBus
reviewed: 2025-08-13
component: Core
related:
- samples/asyncapi/custom-message-types
---

This sample demonstrates how to generate an [AsyncAPI](https://www.asyncapi.com/en) schema document from NServiceBus endpoints using [Neuroglia.AsyncApi](https://github.com/asyncapi/net-sdk).

> [!NOTE]
> AsyncAPI integration is not an officially supported feature of NServiceBus, and this example is for demonstration purposes only.

## Code walk-through

This sample contains four projects:

- [AsyncAPI.Feature](#code-walk-through-feature-project) - a class library containing shared code required to enable the custom `AsyncApiFeature`
- [Shared](#code-walk-through-shared-project) - a class library defining the events that are being published and subscribed to
- [AsyncAPI.GenericHost](#code-walk-through-generic-host-project) - a console application that publishes two events, and generates an AsyncAPI document schema for its structure and saves it to disk
- [Subscriber](#code-walk-through-subscriber-project) - a console application that subscribes to the events published by AsyncAPI.GenericHost

### Feature project

The project contains all the necessary code for registering a custom AsyncAPI document schema generator.

The `EndpointConfigurationExtensions` enables the `AsyncApiFeature`.

snippet: EnableAsyncApiSupport

#### AsyncAPI feature

The feature creates a list of events and registers them in the container so that they are available to be used by the [AsyncAPI document generator](#code-walk-through-feature-project-asyncapi-feature-asyncapi-document-generator).

snippet: RegisterEvents

Finally, the code registers a custom implementation of the Neuroglia `IAsyncApiDocumentGenerator` which will be used instead of the default implementation to generate the NServiceBus-specific schema document.

snippet: RegisterCustomDocumentGenerator

##### AsyncAPI document generator

This custom implementation of the Neuroglia `IAsyncApiDocumentGenerator` creates one AsyncAPI schema document for the NServiceBus endpoint hosted in the application. It demonstrates how channel information with the endpoint's address (queue name) can be generated, containing the publish operation and the payload of the event being published.

snippet: GenerateChannelsForEvents

> [!NOTE]
> This will get all events in the project - some may not be published by this endpoint, others may only be subscribed to. If that's the case, an extra filter would need to be added to differentiate the events.
> Look at the [AsyncAPI with custom message conventions sample](/samples/asyncapi/custom-message-types) to see how this can be addressed.

This code can be extended to include subscriptions, as well as sent/received messages.

### Shared project

The project does not have any references to AsyncAPI. It contains two events that are published by the `AsyncAPI.GenericHost` application and subscribed to by the `Subscriber` endpoint.

### Generic host project

#### Setup AsyncAPI

The project enables the AsyncAPI schema generation using three setup calls.

First, by adding the Neuroglia AsyncAPI.

snippet: GenericHostAddNeurogliaAsyncApi

Second, by adding the AsyncAPI feature to the NServiceBus endpoint.

snippet: GenericHostEnableAsyncAPIOnNSB

Lastly, by adding a background service to generate and write the AsyncAPI document schema to disk.

snippet: GenericHostAddSchemaWriter

#### Access AsyncAPI schema document

The `AsyncAPISchemaWriter` uses the custom document generator injected as part of the `AsyncApiFeature` to generate the document schema and write it to disk.

snippet: GenericHostCustomGenerationAndWritingToDisk

The file is saved into the default `Downloads` folder - it can be viewed using the [AsyncAPI Studio](https://studio.asyncapi.com/) (by pasting in the contents) and incorporated into internal system documentation.

### Subscriber project

The project does not have any references to AsyncAPI. It contains handlers for events that it is subscribing to.

## Running the sample

When running the solution with Visual Studio, two applications will start automatically.

> [!NOTE]
> If using a different IDE or the .NET CLI, start each project individually to run both applications.

### AsyncAPI.GenericHost (console)

- Press `1` to publish `SampleEventOne` - received by `Subscriber`.
- Press `2` to publish `SampleEventTwo` - received by `Subscriber`.

### Subscriber (console)

Displays all events published from `AsyncAPI.GenericHost`.
