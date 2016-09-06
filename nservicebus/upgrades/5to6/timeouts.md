---
title: Timeout changes in Version 6
tags:
 - upgrade
 - migration
related:
 - nservicebus/sagas/timeouts
---


## Timeout storage

`IPersistTimeouts` has been split into two interfaces, `IPersistTimeouts` and `IQueryTimeouts`, to properly separate those storage concerns. Both must be implemented to have a fully functional timeout infrastructure.

`IQueryTimeouts` implements the concern of polling for timeouts outside the context of a message pipeline. `IPersistTimeouts` implements the concern of storage and removal for timeouts which is executed inside the context of a pipeline. Depending on the design of the timeout persisters, those concerns can now be implemented independently. Furthermore, `IPersistTimeouts` introduced a new parameter `TimeoutPersistenceOptions `. This parameter allows access to the pipeline context. This enables timeout persisters to manipulate everything that exists in the context during message pipeline execution.


## Automatic retries

Previously configuring the number of times a message will be retried by the First Level Retries (FLR) mechanism also determined how many times the `TimeoutManager` attempted to retry dispatching a deferred message in case an exception was thrown. From Version 6, the `TimeoutManager` will attempt the dispatch five times (this number is not configurable anymore). The configuration of the FLR mechanism for non-deferred message dispatch has not been changed.