---
title: Fluent Validation
summary: Using NServiceBus.FluentValidation to validate properties on incoming and outgoing messages
reviewed: 2018-08-28
component: FluentValidation
---

Uses the [NServiceBus.FluentValidation](/nservicebus/messaging/validation-fluentvalidation.md) component to validate various properties of the incoming message and outgoing messages.


## Code walk-through

### Adding the fluent message validator

In the snippet below, a fluent `AbstractValidator` for the type `MyMessage` is created with a rule to check that the `Content` property of the message is not empty. 

snippet: Fluent-Validator


### Configuring the endpoint

The validator is then registered in the endpoint configuration as shown in the snippet below. The validation used in this sample is for incoming messages only, so the validator configuration is disabled for outgoing messages. Validators are scanned on start up and [registered and resolved using the container](/nservicebus/messaging/validation-fluentvalidation.md#validator-scanning).

snippet: Enable


## Running the project

 1. Start the solution.
 1. Two messages of type, `MyMessage` are automatically sent to the endpoint on start up. The first message has a value  for the `Content` property and the second message has an empty value.
 1. The handler for the message whose `Content` property has been set will be handled correctly while a validation exception will be thrown for the message whose `Content` property is empty.



