---
title: Fluent Validation
reviewed: 2018-08-28
component: FluentValidation
---

## Introduction

Uses the [NServiceBus.FluentValidation](/nservicebus/messaging/validation-fluentvalidation.md) component to validate various attributes of the incoming message and outgoing messages.


## Code Walk-through


### Adding the fluent message validator

In the snippet below, a fluent `AbstractValidator` for the type `MyMessage` is created with a rule to check that the `Content` attribute of the message is not empty. 

snippet: Fluent-Validator


### Configuring the endpoint

The validator is then registered in the endpoint configuration as shown in the snippet below. The validation used in this sample is for incoming messages only, hence the validator configuration is disabled for outgoing messages. Validators are scanned on start up and [registered and resolved using the container](/nservicebus/messaging/validation-fluentvalidation.md#validator-scanning).

snippet: Enable


## Running the project

 1. Start the solution.
 1. Two messages of type, `MyMessage` are automatically sent  to the endpoint on start up. The first message has a valid content for its attribute and the second message's `Content` attribute is empty.
 1. The handler for the message whose `Content` attribute has a value will be handled correctly while a validation exception will be thrown for the message whose `Content` attribute is empty.



