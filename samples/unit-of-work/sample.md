---
title: Unit of Work Usage
summary: How to create a custom unit of work
reviewed: 2025-05-30
component: Core
related:
 - nservicebus/pipeline/unit-of-work
---

> [!WARNING]
> `IManageUnitOfWork` is obsolete as of NServiceBus version 9. [Use a pipeline behavior to manage a unit of work instead.](/samples/pipeline/unit-of-work/)

This sample demonstrates how to implement a custom [unit of work](/nservicebus/pipeline/unit-of-work.md).

include: uow-access-to-context

1. Run the solution.
1. Press <kbd>s</kbd> to send a message that succeeds.
1. Press <kbd>t</kbd> to send a message that throws an exception.

## Code walk-through

[Immediate retries](/nservicebus/recoverability/configure-immediate-retries.md) and [delayed retries](/nservicebus/recoverability/configure-delayed-retries.md) are disabled to avoid excessive error logging in the console.

### CustomManageUnitOfWork

Logs both the start and end of the unit of work.

snippet: CustomManageUnitOfWork

### Component registration

Registers the custom unit of work.

snippet: componentRegistration

### SuccessHandler

Logs when a message is successfully received.

snippet: SuccessHandler

### ThrowHandler

Logs when a message is received, then throws an exception.

snippet: ThrowHandler
