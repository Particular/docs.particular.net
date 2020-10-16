---
title: Data distribution
summary: Why asynchronous messaging is not a good fit for data distribution scenarios
reviewed: 2020-09-01
redirects:
- nservicebus/azure/data-distribution
- samples/routing/data-distribution
- nservicebus/messaging/data-distribution
---

Data distribution refers to a pattern where asynchronous messages (e.g. using NServiceBus) are used to deliver data to multiple targets in some coordinated fashion.

A common example of a data distribution scenario is having cached data on multiple scaled-out web servers and attempting to deliver a message to each of them. The message indicates each server should drop their current cache entries and retrieve fresh data from the database.

## Timing and reliability

The goal of data distribution is generally to cause a change in data and/or behavior in multiple locations at the same time. NServiceBus does not provide any facility for receiving or processing messages in multiple locations at the same time. There is no urgency to act _now_ in reaction to a message sent with NServiceBus. Asynchronous messages are intended for eventual consistency. Each receiver processes a given message whenever it happens to receive that message from the queue.

In the worst-case scenario, some receivers of a data distribution message may succeed in handling the message, and others may fail, causing the message to be moved to the error queue. Until an administrator diagnoses the failure and returns the message to its source queue, transactions may be handled very differently by nodes that had successfully handled the message and those that had not.

These kinds of failures are difficult to track down. They depend upon more than the contents of a message and the code handling it. The results are also dependent upon the state of the data being handled by data distribution, which will not be consistent across all servers in a cluster.

## Differences from publish/subscribe

Data distribution employs a broadcast distribution strategy, delivering information to all physical nodes. This is different to publish/subscribe, which is designed to reliably deliver a message to one logical endpoint only.

When a message processing endpoint is not scaled out, there is no difference. However, when scaled out, multiple physical endpoint _instances_ together make up one logical endpoint. The instances cooperate to process messages from the same input queue.

With publish/subscribe, a message is routed to a _logical_ endpoint, and it makes no difference which physical endpoint instance ends up processing it.

A data distribution operation is a _broadcast_ operation, which is logically different to a _publish_ operation. While several publish operations can be subverted to act like a broadcast operation, it is not recommended. Publish/subscribe is a poor fit for data distribution. For these reasons, data distribution is not supported by NServiceBus.

## Recommendations

Asynchronous messaging (e.g. NServiceBus) is **not** a good solution for data distribution scenarios. It is usually better to use a dedicated data distribution technology, such as a distributed cache or distributed configuration service. These kinds of technologies are difficult to implement because of the inevitable race conditions and consensus issues but several have evolved that solve these problems elegantly.

Distributed caches, such as [Redis](https://redis.io/), [NCache](https://www.alachisoft.com/ncache/), or [Memcached](http://memcached.org/) can provide fast, local data caching with built-in replication. They can be treated as one global system, rather than separate caches synchronized by exchanging messages.

Distributed configuration services, such as [Apache Zookeeper](https://zookeeper.apache.org/), [Consul](https://www.consul.io/), and [etcd](https://etcd.io/) provide sophisticated consensus algorithms allowing seamless distribution of configuration data and service discovery data, minimizing deployment complexity.

These types of technologies should be used to provide data distribution capabilities instead of asynchronous messaging.
