---
title: DataAnnotations Message Validation
summary: Using NServiceBus.DataAnnotations to validate properties on incoming and outgoing messages
reviewed: 2018-10-28
component: DataAnnotations
---

Uses the [NServiceBus.DataAnnotations](/nservicebus/messaging/validation-dataannotations.md) component to validate various properties of the incoming message and outgoing messages.


## Code walk-through

### Annotating the message type

In the snippet below, for the type `MyMessage`, the `Content` property of the message is marked with a `Required` attribute defined in the `System.ComponentModel.DataAnnotations` namespace. 

snippet: DataAnnotations-Validator


### Configuring the endpoint

The endpoint is then configured to enable the DataAnnotations validator using the snippet below:

snippet: Enable


## Running the project

 1. Start the solution.
 1. Two messages of type, `MyMessage` are automatically sent to the endpoint on start up. The first message has a value for the `Content` property and the second message has an empty value.
 1. The handler for the message whose `Content` property has been set will be handled correctly while a validation exception will be thrown for the message whose `Content` property is empty.