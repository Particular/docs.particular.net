---
title: Unit of Work Usage
reviewed: 2016-11-08
component: Core
related:
 - nservicebus/pipeline/unit-of-work
---


This sample shows how to create a custom [Unit Of Work](/nservicebus/pipeline/unit-of-work.md).


 1. Run the solution.
 1. Press 's' to send a message that will succeed. Press `t` to send a message that will throw.


## Code walk-through

[Immediate Retries](/nservicebus/recoverability/configure-immediate-retries.md) and [Delayed Retries](/nservicebus/recoverability/configure-delayed-retries.md) have been disabled to reduce the amount of error logged to the console.


### CustomManageUnitOfWork

The Unit Of Work logs both the begin and end.


snippet:CustomManageUnitOfWork

### Component Registration

snippet: componentRegistration


### SuccessHandler

The `SuccessHandler` logs the fact that a message has been received.

snippet:SuccessHandler


### ThrowHandler

The `SuccessHandler` logs the fact that a message has been received, and then throws an exception

snippet:ThrowHandler