---
title: Understanding transactions in Windows Azure
summary: Understanding what kind of transactions are supported in Windows Azure and how we deal with this in NServiceBus.
originalUrl: http://docs.particular.net/articles/hosting-nservicebus-in-windows-azure
tags: []
---

The Windows Azure Platform and NServiceBus make a perfect fit. On the one hand the azure platform offers us the scalable and flexible platform that we are looking for in our designs, and on the other hand it is NServiceBus that makes development on this highly distributed environment a breeze. However, there are a few things to keep in mind when developing for this platform, the most important being, the lack of (distributed) transactions. 

To better understand why this feature is lacking, let's first take a closer look at the implications of these technologies.

Understanding Transactions
--------------------------

A transaction comprises a unit of work performed within a database management system (or similar system) against a database, and treated in a coherent and reliable way independent of other transactions. A database transaction must be atomic, consistent, isolated and durable. Transactions provide an "all-or-nothing" proposition, stating that each work-unit performed in a database must either complete in its entirety or have no effect whatsoever.

I assume most of you already knew what a transaction was. But what is often overlooked, is that in order to guarantee isolation, the database engine must lock certain records in use during the transaction, depending on isolation level, so that other transactions cannot work with them at the same time. 

Such locks are all fine and dandy when the database management system and client know and trust each other. But in a cloud or self-service environment, that trust level is simply not there. The windows azure platform must assume that you are a malicious user that is trying to perform a denial of service attack on the platform and is thus very hesitant to let you control all the locks by means of a transaction. 

This is the primary reason why many windows azure hosted services simply do not support transactions at all or are very aggressive when it comes to lock duration. 

For example:

* Windows azure storage services has no support for transactions
* Windows azure database supports local transactions, but only grants locks on resources, when required by a system task for 20 seconds, and 24 hours otherwise. See [this msdn article](http://msdn.microsoft.com/en-us/library/windowsazure/dn338081.aspx#TransactionDurationLimit) for more details.

When both the database management system and client are under the same ownership, imagine you just deployed Sql Server to your own Virtual Machine, this is no longer an issue and you can control the lock duration. But even in this case, you need to be careful when it comes to distributed transactions. 

Understanding Distributed Transactions and the 2 phase commit protocol
----------------------------------------------------------------------

When multiple transaction aware resources are involved in a single transaction, than this transaction will automatically promote to a distributed transaction. That means that handling the unit of work is now performed outside the database system by a so called Global Transaction Manager, or Distributed Transaction Coordinator (DTC). This coordinator, the DTC service on the machine where the transaction started, will communicate with similar services on the machines involved by means of the 2 phase commit protocol, called resource managers.

As illustrated in the diagram below, the 2 phase commit protocol consists of 2 phases where the global transaction manager communicates with all other resource managers on order to coordinate the transaction. During the prepare phase it instructs all resource managers to get ready to commit and when all resource managers approve (or not) it will instruct all resource managers again to complete the commit (or rollback).

![2 Phase Commit](2PC.jpg)

Note, that this protocol requires 2 communication steps for each resource manager added to the transaction and it also requires a response of each of them to be able to continue. Both of these conditions are problematic in a huge datacenter such as the windows azure ones.

* 2 communication steps per added resource manager will result in an exponential explosion of communication. 2 resources = 4 network calls, 4 = 16, 100 = 10000, etc...
* Requirement to wait for all responses: As the windows azure datacenters are huge, check out [this video (5 mins in)](http://www.youtube.com/watch?v=JJ44hEr5DFE) to get an idea how huge, it's very likely that network partitioning will occur in your solution as virtual machines are physically remote from each other, network infrastructure will die, resulting in slow or in doubt transactions being more common than in a small network.

This is the reason why none of the windows azure services supports distributed transactions, and that you are encouraged not to use distributed transactions either even if you technically could.

Side note: The .net framework will promote to a distributed transaction rather quickly, for example 2 open connections to the same resource (exact same connectionstring), will still promote to a distributed transaction, and there is no option to disable promotion as far as I know of. 

How to use NServiceBus in this environment
-------------------------------------------------

By default, NServiceBus relies on the DTC to make distributed system development really easy. But in the windows azure environment, you should (or cannot) use the DTC. So we have to configure/use it a bit differently. 

There are quire a few options though, in the remainder of this article we'll discuss each option with it's pro's and it's cons. Depending on your scenario you may choose to configure NServiceBus differently.

The options are:

* Share local transaction
* Unit of work with batching and transport retries
* Atomic operations with transport retries
* Saga's and compensation logic
* Routing slips and compensation logic

### Share local transactions

Prevent transaction promotion by reusing a single local transaction. The idea is to inject the outer most local transaction/unit of work, started by the receiving transport, into the rest of the application (like your orm etc.) so that only a single transaction is used for both receiving and handling a message and it's result.

**Pro's:** 

* By sharing a single local transaction by both your transport and business logic, you prevent the DTC from kicking in while preserving the simple programming that you are used to. Besides injecting the transport level in the rest of the application, nothing changes really.

**Cons:** 

* You are limited to a single transactional resource for your entire system. The technique can only be applied if your application fits the limits of this transactional resource.
* The technology used for communication and storage is by definition the same.
* Must be able and willing to inject the transaction, this may be a challenge when using third party libraries for example.

### Atomic operations or, Unit of work with batching, and transport retries

Both are a variation to the same topic, and the technique is used when one of the resources used does not support transactions at all. The idea is that every single operation is 'transactional' by default, in the sense that the operation either succeeds or fails as a single unit. And if you limit yourself to single operations only, you don't need transactions anymore. Some storage facilities even allow you to group a number of operations into a single batch, to make the set atomic.

However, regular transactions also imply 'rollback' semantics which will make the receive infrastructure retry the original message again later. Therefore, you need to combine this technique with a transport that supports retry semantics.

**Pro's:**

* You don't need transactions at all to provide consistency.
* Get retry on transient exceptions for free.

**Con's**

* You need to change the way you program your business logic to become atomic. Just 1 insert, update or delete at a time, this isn't always easy.
* You need to change your business logic to become idempotent. As message retry is added to the equasion, but outside of the scope of the atomic operation, you need to make sure that the same operation can be executed over and over again without changing the end result. See (the need for idempotency on techniques to achieve this)
* Retry behavior is typically combined with timeouts, that will kick in not only if your operation failed, but also when it is to slow. This can lead to situation where the same operation is executed multiple times in parallel.

### Saga's and compensation logic

Saga's in NServiceBus are a state full set of message handlers that can be used to track and orchestrate the different parts of a transaction. They communicate with other handlers that each perform part of the transaction and acknowledge when the work succeeded or failed, and upon those results they decide what needs to happen to the rest of the transaction, continue or make things right again. The latter is often called compensation logic, as it tries to compensate at a business logic level to deal with failures. In essence you write a distributed transaction coordinator built with business logic, instead of the 2 phase commit protocol, yourself.

**Pro's:** 

*  You don't need transactions at all to provide consistency.
*  Extremely flexible and maps very well with the business domain.

**Con's:**

* Involves quite a lot of work to think of, and implement, all possible variations and error conditions in a transaction.
* Requires a good understanding of the business to implement effectively.
* If applied in conjunction with atomic operations that cannot be batched, you need to keep in mind that the saga is state full as well and therefore breaks the atomicity of the operation. This needs to be taken into account when designing the saga, you should never execute operations against a store inside the saga itself, but always use another message handler and a queue in between and cater for idempotency at all levels.

### Routing slips and compensation logic

Where the saga approach uses a central coordinator that keeps track and orchestrates the work in a transaction, this approach uses a chain of independent handlers instead. You can think of it as a linked list of message handlers that can send messages back and forward in the list. The first handler composes a 'routing slip' that contains all the work that needs to be done in the transaction and forwards that to the next handler in the chain. This handler executes it's part and in case of success forwards the message to the next one in the chain until all handlers have been invoked and the transaction completed. If at any point in time, a handler fails to complete it's logic, it will send the message back in the chain to execute compensation logic to make things right again. The slip is continuously sent back until it reaches the original handler again and the transaction is considered rolled back.

**Pro's**

*  You don't need transactions at all to provide consistency.
*  Is more 'linear' in conceptual model than saga's, so may be easier to keep in your mind.
*  Does not require to maintain state in a data store, it's implicit by the chaining of handlers and passing around the routing slip (to which state can be added for the next handler in the list)

**Con's**

*  Involves quite a lot of work to think of, and implement, all possible variations and error conditions in a transaction.
*  Less flexible than Saga's, requires linear thinking, can't execute handlers in parallel and therefore often slower.


The need for idempotency
------------------------

Note that every approach that involves retries, will result in at least once delivery semantics, or in other words, you can get the same message multiple times. You need to take this into account when designing your business logic and ensure that every operation is idempotent, or can be executed multiple times.

There are multiple ways to deal with idempotency though, some at the technical level, others built into the business logic itself. 

Depending on your business needs you can go for one of these:

* Message deduplication
* Natural idempotency
* Entities and messages with version information
* Side effect checks
* Partner state machines
* Accept uncertainty

### Message deduplication

This is probably the easiest way to detect if a message has been executed already, you simply store every message that has been processed so far and when a new message comes, in you compare it to the set of already processed messages. If you find it in the list, you have processed it before.

The approach has obvious downsides as well. As every message needs to be stored and searched for, it can reduce message throughput because of the lookup requirement and potentially cause contention on the message store as well at high volumes.

### Natural idempotency

Many operations can be designed in a naturally idempotent way, `TurnOntheLights` is by default an idempotent operation, it will have the same effect no matter if the lights were on or off to begin with and no matter how often you execute it. `FlipTheLightSwitch` however is not naturally idempotent, the results may vary on the start condition and the number of times you execute it. Try to make use of natural idempotency as much as possible.

### Entities and messages with version information

Another technique is to add versioning information to your entities (timestamp or versionnumber or the likes) and include that version information whenever a command is sent that would alter the state of said entity. Now the handling logic can compare the versioning information on both the entity and the message and decide whether this logic needs to be executed or not. 

The downside of this approach is that the version of the entity can change for different commands, and may therefore cause unexpected outcomes when unrelated commands arrive in a different order than logically sent.

### Partner state machines

A better approach is to organize the state inside an entity on a per partner basis, as a miniature state machine. Ultimately the only non-idempotent messages occur when one entity issues a command to another, and if you follow the `one master` rule, there should only be one such logical endpoint sending that command. Therefore if you organize your state internally in the entity according to that relationship, and keep track of the progression of that relationship as a state machine, it's impossible to have versioning conflicts.

### Side effect checks

Arguably a dangerous approach, but often very useful in the real world, is to check for indirect side effects of a command to determine if a it needs to be processed or not. When `TheFireIsHot` there is no need to `TurnOnTheFire` anymore.

### Accept uncertainty

And finally, there is always the option, from business perspective, to live with some uncertainty and potentially wrong data caused by non-idempotent messages. Maybe it just doesn't matter that much, an occasional retrying `+1` operation may not be that important when your counter is already on 8,489,232,123. Or maybe there re ways to deal with inconsistencies afterwards, that's why credit notes were invented right. It's hard for us developers to accept this, but the real world works without any form of transaction or idempotency management.

























