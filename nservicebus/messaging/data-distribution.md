---
title: Data distribution
summary: Why asynchronous messaging is not a good fit for data distribution scenarios
reviewed: 2020-05-21
redirects:
- nservicebus/azure/data-distribution
- samples/routing/data-distribution
---

Data distribution refers to a pattern where asynchronous messages (i.e. using NServiceBus) are used to deliver data to multiple targets in some coordinated fashion.

A common example of data distribution scenario is having cached data on multiple scaled-out web servers, and then attempting to use messages delivered to all web servers in a cache-busting fashion, so that each web server drops its current cache entries, and then is forced to retrieve fresh data from the database.

## Timing and reliability

The goal of data distribution is generally to cause a change in data and/or behavior in multiple locations at the same time. Messages sent with NServiceBus, however do not care if they are received or processed at the same time. There is no urgency to act _now_ with a message sent with NServiceBus. Because asynchronous messages are intended for eventual consistency, each receiver would be free to process it at their leisure, whenever it gets around to handling that message in the queue.

In the worst-case scenario, some receivers of a data distribution message might succeed, while others encounter an unknown error causing the message to be moved to the error queue. In the time until an administrator is able to diagnose the failure and return the message to its source queue, transactions would be handled very differently by nodes that had received the message and those that had not.

These sorts of failures are difficult to track down, as they depend upon more than the contents of a message and the code processing it. The result would also be dependent upon the state of the data being handled by data distribution, which would not be consistent between different servers in a cluster.

## Differences from publish/subscribe

Data distribution employs a broadcast distribution strategy, delivering the information to all physical nodes. This is different than a publish/subscribe scenario, which is designed to reliably deliver a message to only one logical endpoint.

In a system in which a message processing endpoint is not scaled out, there is no difference. But when scaled out, multiple physical endpoint _instances_ together make up one logical endpoint, all cooperating to process messages from the same input queue.

In a publish/subscribe scenario, messages are routed to a _logical_ endpoint, and it makes no difference which physical endpoint instance ends up processing it.

A data distribution operation is a _broadcast_ operation, which is logically different than a _publish_ operation. While a publish operation can be subverted to act like a broadcast operation, it is not recommended to try to shoehorn one into the other, so this capability is not supported by NServiceBus.

## Recommendations

Asynchronous messaging (i.e. NServiceBus) is **not** an optimal solution for data distribution scenarios. It is usually better to use a dedicated data distribution technology, such as a distributed cache or distributed configuration service. These kinds of services are difficult to implement because of the inevitable race conditions and consensus issues. Dedicated tools have sprung up that solve these problems and do it well.

Distributed caches, such as [Redis](https://redis.io/), [NCache](https://www.alachisoft.com/ncache/), or [Memcached](http://memcached.org/) can provide fast, local data caching with built-in replication. They solve the problems above efficiently, which enables thinking of the cache as one global service, rather than trying to synchronize many disparate caches by exchanging messages.

Distributed configuration services, such as [Apache Zookeeper](https://zookeeper.apache.org/), [Consul](https://www.consul.io/), and [etcd](https://etcd.io/) provide sophisticated consensus algorithms that allow the seamless distribution of configuration data and service discovery data, minimizing deployment complexity.

One of these types of tools should be selected to provide data distribution capabilities instead of asynchronous messaging.
