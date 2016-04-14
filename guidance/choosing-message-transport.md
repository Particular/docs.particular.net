---
title: Choosing Message Transport
summary: Guidance for choosing a message transport for NServiceBus.
---

The single most important decision to make when architecting an NServiceBus solution is the choice of message transport. A message transport includes an underlying queueing architecture as well as any of the libraries needed for NServiceBus to interact with it.

While every attempt is made to make each message transport functionally identical, some quirks of each must invariably leak through, and in addition, there are various technical and organizational factors to consider when selecting a transport.

## MSMQ

* Out of the box, meaning it's included in Windows
* Store & forward default mode
  * Network isn't reliable
  * Describe how store & forward works
  * No single point of failure
* DTC
  * Two or more network hosts
  * ACID - all-or-nothing outcomes
  * Chatty, slower as you enlist more and more resource managers
  * If one has a latency issue, all or nothing means that the whole thing comes down
  * Tx duration = max(individual tx) + overhead, so can lead to locks being taken for longer, increased likelihood of deadlocks
* Reality w/ NServiceBus
  * Only two nodes: App node (Client + MSMQ) and DB node (SQL) - not much overhead
  * Transaction length defined by your SQL statements against your SQL Server with queries you optimize
* Code focused on the business problem (DB.Store + Bus.Publish)
  * No worries about idempotency or ghost messages.
* Non-transactional things
  * Send another messages (DB.Store + Bus.Send(ChargeCreditCard))
* Getting DTC set up is non-trivial
  * Once working, it just works
  * Keep calm and use DTCPing.exe
* All about tradeoffs by relying on DTC
  * Time to market (getting code to production quickly with less bugs) vs. Environment lock-in (Windows, recoding)
* MSMQ hasn't been updated in forever, but essentially a "finished" product

## RabbitMQ

* Because Erlang
* Integrate all the things
  * Client libraries for every programming language imaginable
  * AMQP compliant
* No transactions
  * Order of operations matters
    * NSB supports multiple handlers so ordering of statements not always so easy to see
  * If store then publish, OK as long as store rolls back
  * Ghost messages: if publish then store, ghost message goes out anyway
* Dealing with dupes
  * Try store with deterministic/client-side ID, catch Key constraint exception, then publish (another potential dupe)
  * Updates harder: Have to track what messageIds have not been handled
* Infrastructure-level idempotency via Outbox
  * Must use same database as business data to piggyback tx
* High-availability
  * Most go with clustering (can do shovel plugin to act more like MSMQ)
  * "RabbitMQ clusters do not tolerate network partitions well." (Rabbit docs)
* Master/slave HA queues
  * 3 nodes behind a load balancer
  * Each queue created has a master node, data replicated to other nodes
  * Always a 66% chance of having to redirect to a different master node, extra network hop
  * Split brain due to cluster partitions
    * If Node A separated by network partition, it will elect itself master, not knowing about the others
    * Node C will still believe it is the master
    * Writing on both sides of cluster
    * Do not use Auto-Heal mode! (not on by default) When partition heals will pick minority, remove all messages, and reapply. Will lose 33% of the messages coming in.
    * Google "Call me maybe RabbitMQ"
    * Need to detect this, pull msgs out of Node A, and reapply them to other nodes manually
    * If you don't want to lose messages, you need to be REALLY available.
    * Involve professionals: Pivotal and others
* Q for Andreas: Linux only or OK on Windows now?
  

## SQL Server

* For monolithic database apps, can use to integrate and get events out
* "No one has been fired for choosing SQL Server"
* Can write triggers to insert messages into queues
* Bridging transports - multi-deserialization (not sure it belongs in this article)
* Comfortable
  * Can start using queuing without introducing new infrastructure (no bike-shedding queue selection)
  * High-availability already in place
  * Backups just work - can get a consistent snapshot containing queues and business data
* Downside: Expen$ive
  * $caling up
  * $QL Server Always On: Perhaps the most expensive checkbox in the world
    * You will lose DTC, but reintroducing in 2016
* If you're a .NET shop using SQL Server and you don't know what queuing system to use, could go with SQL

## Azure

While sometimes lumped together, Windows Azure provides two products that can be used with NServiceBus, Azure Storage Queues and Azure Service Bus.

* Platform as a Service: Infrastructure run by the Other Guys
  * When things happen, you are not in control
  * But you don't have to dedicate resources (except a credit card) to making it go
* Benefit: Credit card driven development - "Shitty code is expensive code"
* Global operations just work
  * As long as you think about
    * How to partition stuff
    * Legal, where to store data, privacy, etc.
    * Security
    * Latency
* Fallacy #2 - Latency is unfortunately not zero
  * Orders of magnitude higher than in same datacenter
  * Chatty is bad - bring as much data as you can with you
  * Batch things if possible
* Batched dispatch in V6 (does this belong in this article?)
  * For all transports. In Azure only available in the async API
  * V6 kills ghost messages

### Azure Storage Queues

* Basics
  * Just a queue
  * Stable from API perspective
  * Base service of a lot of other Azure services, when they need to shuttle data around

### Azure Service Bus

* Advanced features for integration
  * supports relaying
  * AMQP 1.0 compliant
  * integrates with event hubs
  
  
## Summary

* What should I use? It depends. Do the research.
* Don't select based on performance, message size limits.
* Select based on current situation and where you want to go in the future.
* Consider how much environment lock-in I can tolerate now, and how much work to transition later?
* Can use more than one. 
