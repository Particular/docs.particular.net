---
title: NServiceBus Testing Upgrade Version 6 to 7
summary: Instructions on how to upgrade NServiceBus.Testing Version 6 to 7.
reviewed: 2021-07-28
component: Testing
related:
 - nservicebus/testing
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

## Upgrade to NServiceBus Version 7

NServiceBus.Testing requires NServiceBus Version 7.

When upgrading to NServiceBus.Testing Version 7, projects will also require an upgrade to [NServiceBus Version 7](/nservicebus/upgrades/6to7/).

## AssertSagaCompletionIs

The `AssertSagaCompletionIs` method has been obsoleted and replaced by `ExpectSagaCompleted` and `ExpectSagaNotCompleted`.

snippet: 6to7-ExpectSagaCompleted

Note the `ExpectSagaCompleted` and `ExpectSagaNotCompleted` expectations must be placed **before** a `When` method, similar to other expectation methods.

## ExpectHandleCurrentMessageLater

The `ExpectHandleCurrentMessageLater` method has been obsoleted as `IMessageHandlerContext.HandleCurrentMessageLater()` has been deprecated in NServiceBus Version 7.

## WhenHandling

An overload of the `WhenHandling` method has been added, which accepts a preconstructed message.

snippet: 6to7-WhenHandling

## Fluent-style tests deprecated

The fluent-style testing API has been deprecated. See [upgrade guide](/nservicebus/upgrades/testing-7to8.md) for help migrating to Arrange-Act-Assert (AAA) test API.