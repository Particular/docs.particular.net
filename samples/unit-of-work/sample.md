---
title: Unit of Work Usage
summary: How to create a custom unit of work
reviewed: 2020-06-18
component: Core
related:
 - nservicebus/pipeline/unit-of-work
---


This sample shows how to create a custom [unit of work](/nservicebus/pipeline/unit-of-work.md).

include: uow-access-to-context

 1. Run the solution.
 1. Press <kbd>s</kbd> to send a message that will succeed. Press <kbd>t</kbd> to send a message that will throw.


## Code walk-through

[Immediate retries](/nservicebus/recoverability/configure-immediate-retries.md) and [delayed retries](/nservicebus/recoverability/configure-delayed-retries.md) have been disabled to reduce the number of errors logged to the console.


### CustomManageUnitOfWork

The unit of work logs both the begin and end.


snippet: CustomManageUnitOfWork

### Component registration

snippet: componentRegistration


### SuccessHandler

The `SuccessHandler` logs when a message has been received.

snippet: SuccessHandler


### ThrowHandler

The `ThrowHandler` logs when a message has been received, then throws an exception

snippet: ThrowHandler
