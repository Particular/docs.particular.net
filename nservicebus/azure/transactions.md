---
title: Transactions in Azure
summary: Understanding what kind of transactions are supported in Azure and how we deal with this in NServiceBus.
tags: 
- Azure
- Cloud
- Transaction
- Idempotency
- DTC
redirects:
 - nservicebus/understanding-transactions-in-windows-azure
---

The Azure Platform and NServiceBus make a perfect fit. On the one hand the Azure platform offers the scalable and flexible platform that you are looking for in your designs, and on the other hand NServiceBus makes development on this highly distributed environment a breeze. However, there are a few things to keep in mind when developing for this platform, the most important being the lack of (distributed) transactions. 

To better understand why this feature is lacking, let's examine the implications of these technologies.


## Understanding transactions

Transaction processing is designed to maintain systems integrity (typically a database or some modern filesystems and services) in a known, consistent state, by ensuring that interdependent operations on the system are either all completed successfully or all canceled successfully. This article mostly considers database technology and storage services.

What is often overlooked in transactional processing, especially in the context of cloud services, is that to guarantee isolation, the database engine must lock certain records in use during the transaction, depending on isolation level, so that other transactions cannot work with them at the same time.

Such locks become a trust issue in a cloud or self-service environment, as external parties can use these locks to perform a denial of service attack. The Azure platform must assume that you are a malicious user and is thus very hesitant to let you control all the locks by means of a transaction. 

This is the primary reason why many Azure hosted services do not support transactions at all or are very aggressive when it comes to lock duration. 

For example:

* Azure storage services have no support for transactions. This is not explicitly documented but you can find enough [references on stackoverflow](http://stackoverflow.com/questions/18045517/do-azure-storage-related-apis-participate-in-system-transactions)
* The Azure database supports local transactions, but only grants locks on resources, when required by a system task for 20 seconds, and 24 hours otherwise. See [this msdn article](https://msdn.microsoft.com/library/azure/dn338081.aspx#TransactionDurationLimit) for more details.

When both the database management system and client are under the same ownership, imagine you just deployed SQL Server to your own virtual machine, so locks are no longer an issue and you can control the lock duration. But even in this case, you need to be careful when it comes to distributed transactions. 


## Understanding distributed transactions and the two-phase commit protocol

When multiple transaction-aware resources are involved in a single transaction, then this transaction automatically promotes to a distributed transaction. That means that handling the unit of work is now performed outside the database system by the so-called Global Transaction Manager, or Distributed Transaction Coordinator (DTC). This coordinator, the DTC service on the machine where the transaction started, communicates with similar services on the machines involved by means of the two-phase commit protocol, called resource managers.

As illustrated in the diagram below, the two-phase commit protocol consists of two phases where the global transaction manager communicates with all other resource managers to coordinate the transaction. During the preparation phase it instructs all resource managers to get ready to commit and when all resource managers approve (or not), it instructs all resource managers again to complete the commit (or rollback).

![Two Phase Commit](two-phase-commit.png)

Note that this protocol requires two communication steps for each resource manager added to the transaction and requires a response from each of them to be able to continue. Both of these conditions are problematic in a huge datacenter such as Azure.

* Two communication steps per added resource manager results in an exponential explosion of communication. 2 resources = 4 network calls, 4 = 16, 100 = 10000, etc...
* Requirement to wait for all responses: the Azure datacenters are huge. Check out [this video (5 mins in)](https://www.youtube.com/watch?v=JJ44hEr5DFE) to get an idea of how huge. It is very likely that network partitioning will occur in your solution as virtual machines are physically remote from each other, so network infrastructure will die, resulting in slow or in doubt transactions being more common than in a small network.

This is the reason why none of the Azure services supports distributed transactions, and so you are encouraged not to use distributed transactions even if you technically could.

Side note: The .NET framework promotes to a distributed transaction rather quickly; for example, two open connections to the same resource (exact same connectionstring), will still promote to a distributed transaction, and there is no option to disable promotion. 


## How to use NServiceBus in this environment

By default, NServiceBus relies on the DTC to make distributed system development really easy. But in the Azure environment, you cannot use the DTC. So you have to configure/use it a bit differently. 

There are quite a few options. The remainder of this article discusses each option with its advantages and disadvantages. Depending on your scenario you may choose to use NServiceBus differently.

The options:

* Share local transaction
* Unit of work with batching and transport retries
* Atomic operations with transport retries
* Sagas and compensation logic
* Routing slips and compensation logic


### Share local transactions

Prevent transaction promotion by reusing a single local transaction. The idea is to inject the outermost local transaction/unit of work, started by the receiving transport, into the rest of the application (like your orm, etc.) so that only a single transaction is used for both receiving and handling a message and its result.

**Advantages** 

* By sharing a single local transaction by both your transport and business logic, you prevent the DTC from kicking in while preserving the simple programming that you are used to. Besides injecting the transport level in the rest of the application, nothing really changes.

**Disadvantages** 

* You are limited to a single transactional resource for your entire system. The technique can only be applied if your application fits the limits of this transactional resource. As some Azure services throttle quite aggressively, sometimes on behavior of other tenants, capacity planning might become an issue.
* Must be able and willing to inject the transaction, which may be a challenge when using third-party libraries, for example.


### Atomic operations and transport retries

This technique is used when a resource does not support transactions at all. The idea is that every single operation is 'transactional' by default, in the sense that the operation either succeeds or fails as a single unit. And if you limit yourself to single operations only, you don't need transactions anymore. A unit of work pattern with batching is a single operation as well and can therefore be used to emulate a transaction, but with big restrictions. Azure storage services allow you to group a number of operations into a single batch, to make the set atomic, but only on Azure storage tables and only when the partition key for all operations is exactly the same.

However, regular transactions also imply 'rollback' semantics that will make the receive infrastructure retry the original message again later. You need to combine this technique with a transport that supports retry semantics.

**Advantages**

* You don't need transactions at all to provide consistency.
* Get retry on transient exceptions for free.

**Disadvantages**

* You need to change the way you program your business logic to become atomic; just one insert, update, or delete at a time isn't always easy.
* You need to change your business logic to become idempotent. As message retry is added to the equation, but outside of the scope of the atomic operation, you need to make sure that the same operation can be repeatedly executed without changing the end result. See 'the need for idempotency' on techniques to achieve this.
* Retry behavior is typically combined with timeouts that will kick in not only if your operation failed, but also when it is too slow. This can lead to situations where the same operation executes multiple times in parallel.


### Sagas and compensation logic

Sagas in NServiceBus are a stateful set of message handlers that can be used to track and orchestrate the different parts of a transaction. They communicate with other handlers such that each performs part of the transaction and acknowledges when the work succeeds or fails. Depending on those results, they decide what needs to happen to the rest of the transaction, whether to continue or make things right again. The latter is often called compensation logic, as it tries to compensate at a business logic level to deal with failures. In essence you write a distributed transaction coordinator built with business logic, instead of the two-phase commit protocol, yourself.

**Advantages** 

*  You don't need transactions at all to provide consistency.
*  Extremely flexible and maps very well with the business domain.

**Disadvantages**

* To consider and implement all possible variations and error conditions in a transaction, involves quite a lot of work.
* To implement effectively, requires a good understanding of the business.
* If applied in conjunction with atomic operations that cannot be batched, you need to keep in mind that the saga is stateful as well and therefore breaks the atomicity of the operation. This needs to be taken into account when designing the saga. You should never execute operations against a store inside the saga itself, but always use another message handler and a queue in between and cater for idempotency at all levels.


### Routing slips and compensation logic

Where the saga approach uses a central coordinator that orchestrates the work in a transaction, this approach uses a chain of independent handlers instead. You can think of it as a linked list of message handlers that can send messages back and forward in the list. The first handler composes a 'routing slip' that contains all the work that needs to be done in the transaction, and forwards that to the next handler in the chain. This handler executes its part and upon success forwards the message to the next one in the chain until all handlers have been invoked and the transaction complete. If at any point in time, a handler fails to complete its logic, it sends the message back in the chain to execute compensation logic to make things right again. The slip is continuously sent back until it reaches the original handler again and the transaction is considered rolled back.

**Advantages**

*  You don't need transactions at all to provide consistency.
*  Is more 'linear' in its conceptual model than sagas, so may be easier to keep in your mind.
*  Does not require maintaining the state in a data store; it is implicit by the chaining of handlers and passing around the routing slip (to which the state can be added for the next handler in the list).

**Disadvantages**

*  To consider and implement all possible variations and error conditions in a transaction, it involves quite a lot of work.
*  Less flexible than sagas, requires linear thinking, can't execute handlers in parallel, and therefore is often slower.


## The need for idempotency

Note that every approach involving retries will result in delivery semantics at least once. In other words, you can get the same message multiple times. You need to take this into account when designing your business logic and ensure that every operation is idempotent, or can be executed multiple times.

There are multiple ways to deal with idempotency though, some at the technical level, others built into the business logic itself. 

Depending on your business needs you can go for one of these:

* Message deduplication
* Natural idempotency
* Entities and messages with version information
* Side effect checks
* Partner state machines
* Accept uncertainty


### Message deduplication

This is probably the easiest way to detect if a message has been executed already. You store every message that has been processed so far and when a new message comes in, you compare it to the set of already processed messages. If you find it in the list, you have processed it before.

The approach has obvious downsides as well. As every message needs to be stored and searched for, it can reduce message throughput because of the lookup requirement, potentially causing contention on the message store as well at high volumes.


### Natural idempotency

Many operations can be designed in a naturally idempotent way. `TurnOntheLights` is by default an idempotent operation; it will have the same effect no matter if the lights were on or off to begin with and no matter how often you execute it. `FlipTheLightSwitch` however is not naturally idempotent; the results may vary on the start condition and the number of times you execute it. Try to make use of natural idempotency as much as possible.


### Entities and messages with version information

Another technique is to add versioning information to your entities (timestamp or version number or the likes) and include that version information whenever a command is sent that would alter the state of said entity. Now the handling logic can compare the versioning information on both the entity and the message and decide whether this logic needs to be executed or not. 

The downside of this approach is that the version of the entity can change for different commands, and may therefore cause unexpected outcomes when unrelated commands arrive in a different order than logically sent.


### Partner state machines

A better approach is to organize the state inside an entity on a per partner basis, as a miniature state machine. Ultimately the only non-idempotent messages occur when one entity issues a command to another, and if you follow the `one master` rule, there should only be one such logical endpoint sending that command. Therefore if you organize your state internally in the entity according to that relationship, and keep track of the progression of that relationship as a state machine, it's impossible to have versioning conflicts.


### Side effect checks

Arguably a dangerous approach, but often very useful in the real world, is to check for indirect side effects of a command to determine if it needs to be processed. When `TheFireIsHot` there is no need to `TurnOnTheFire` anymore.


### Accept uncertainty

And finally, there is always the option, from a business perspective, to live with some uncertainty and potentially wrong data caused by non-idempotent messages. Maybe it just doesn't matter that much. An occasional retrying `+1` operation may not be that important when your counter is already on 8,489,232,123. Or maybe there are ways to deal with inconsistencies afterwards, and that's why credit notes were invented right. It's hard for us developers to accept this, but the real world works without any form of transaction or idempotency management.
