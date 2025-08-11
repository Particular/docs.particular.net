---
title: AsyncAPI
summary: How to generate an AsyncAPI schema document from NServiceBus
reviewed: 2025-08-08
component: Core
---

This sample demonstrates how to generate an [AsyncAPI](https://www.asyncapi.com/en) schema document from NServiceBus endpoints using [Neuroglia.AsyncApi](https://github.com/asyncapi/net-sdk) in two different hosting environments:

- ASP.NET Core WebAPI
- using the Generic Host

It also shows how a message can be published by one endpoint and subscribed to by another without using the same namespace, therefore decoupling the systems from each other.

## Code walk-through

This sample contains four projects:

- AsyncAPI.Feature - a class library containing shared code required to enable the custom `AsyncApiFeature`
- AsyncAPI.GenericHost - a console application that publishes two events, sends a message and generates an AsyncAPI document schema for its structure and saves it to disk
- AsyncAPI.Web - a ASP.NET Core WebAPI application that publishes two events and generates an AsyncAPI document schema for its structure that is viewable via a url
- Subscriber - a console application that subscribes to the events published by AsyncAPI.GenericHost and AsyncAPI.Web

### Feature project

TODO

### Subscriber project

TODO

### WebAPI project

TODO

### Generic host project

The project enables the AsyncAPI schema generation using three setup calls.

First by adding the Neuroglia AsyncAPI

snippet: GenericHostAddNeurogliaAsyncApi

Second by adding the AsyncAPI feature to the NServiceBus endpoint configuration

snippet: GenericHostEnableAsyncAPIOnNSB

Lastly by adding a background service to generate and write the AsyncAPI document schema to disk

snippet: GenericHostAddSchemaWriter

## Running the sample

- Run the solution. Two console applications and one web application start
- On the `AsyncAPI.GenericHost` console application:
  - press `s` to send a message to itself
  - press `1` to publish `FirstEventThatIsBeingPublished` - see it received in the `AsyncAPI Subscriber` console application
  - press `2` to publish `SecondEventThatIsBeingPublished` - see it received in the `AsyncAPI Subscriber` console application
- On the `AsyncAPI.Web` application
  - use `https://localhost:7198/scalar` to publish the two different events - see them received in the `AsyncAPI Subscriber` console application
  - use `https://localhost:7198/asyncapi` to view the AsyncAPI document schema for the application. This can be exported to a json or yaml file.
