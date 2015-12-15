---
title: Sagas Not Found
summary: How a message is handled when it could be executed by a saga but no saga could be found.
tags:
- Saga
related:
- samples/saga
---

When a message is received that could possibly be handled by a saga, and no existing saga can be found, then an implementation of `IHandleSagaNotFound` executed.

snippet:saga-not-found

Note that the message will be considered successfully processed and sent to the audit queue even if no saga was found. If you want the message to end up in the error queue just throw an exception from your `IHandleSagaNotFound` implementation.

This feature is also useful if compensating actions need to be taken for messages that are handled by the saga which arrive after the saga has been marked as complete.
