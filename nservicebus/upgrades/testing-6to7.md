---
title: NServiceBus Testing Upgrade Version 6 to 7
summary: Instructions on how to upgrade NServiceBus.Testing Version 6 to 7.
reviewed: 2017-09-28
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

As part of upgrading to NServiceBus.Testing Version 7, projects will also require an upgrade to [NServiceBus Version 7](/nservicebus/upgrades/6to7/).


## Testing Sagas


### AssertSagaCompletionIs

The `AssertSagaCompletionIs` method has been obsoleted and replaced by `ExpectSagaCompleted` and `ExpectSagaNotCompleted`. Instead of:

    Test.Saga<MySaga>()
        .WhenHandling<TestSaga.CompleteSagaMessage>()
        .AssertSagaCompletionIs(true);

use the new expectations to write:

    Test.Saga<MySaga>()
        .ExpectSagaCompleted()
        .WhenHandling<TestSaga.CompleteSagaMessage>();

Note how the `ExpectSagaCompleted` and `ExpectSagaNotCompleted` expectations must be placed **before** a `When` method, similar to other expectation methods.