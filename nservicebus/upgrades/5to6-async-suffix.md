---
title: Async Suffix
summary: Describes reasons behind not adding the async suffix to all the methods
---

Starting with NServiceBus Version 6, the public API is fully Async, which includes the `Bus` methods, the message pipeline, message mutators etc. NServiceBus Version 6 has been written to utilize the .NET Async API. All of the operations such as `Send`, `Publish`, `Reply` are 100% async and awaitable. Although the typical Microsoft convention is to name all methods that return a `Task` with an `async` suffix, it is not relevant for the following reasons:

* In the Microsoft API, there is a mix of synchronous and asynchronous methods, and it becomes necessary to differentiate the two. Therefore adding the `async` suffix becomes a necessity. However, in NServiceBus all of the operations are fully async, and there are no synchronous counterparts. 

* Every operation that participates in the sending a message or receiving a message interacts with IO-bound resources like persisters and returns a Task. Adding the `async` postfix to all these operations becomes redundant.
 
* [Changing the operations to be async](/nservicebus/upgrades/5to6-migrate-existing-handlers.md) in itself is a major breaking change which touches every API in NServiceBus. Changing all the method names to add the `async` suffix adds more work when trying to migrate to Version 6, for example, all the renames that would result in changing `Send` to `SendAsync`, `SendLocal` to `SendLocalAsync`, etc. 

* NServiceBus API is not alone in taking this approach. Several other popular OSS libraries like Octokit are also doing this.