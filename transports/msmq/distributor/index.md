---
title: Scaling out with the Distributor
summary: The distributor maintains all the characteristics of NServiceBus but is designed never to overwhelm any of the worker nodes.
reviewed: 2017-06-30
tags:
 - Scalability
redirects:
 - nservicebus/load-balancing-with-the-distributor
 - nservicebus/scalability-and-ha/distributor
 - nservicebus/licensing-and-distribution
 - nservicebus/licensing/licensing-and-distribution
 - nservicebus/scalability-and-ha
 - nservicebus/scalability-and-ha/licensing
 - nservicebus/msmq/distributor
---

The NServiceBus Distributor is similar in behavior to standard [load balancers](https://en.wikipedia.org/wiki/Load_balancing_%28computing%29). It is the key to transparently scaling out message processing over many machines.

As a standard NServiceBus process, the Distributor maintains all the fault-tolerant and performance characteristics of NServiceBus but is designed to never overwhelm any of the worker nodes.

WARNING: Keep in mind that the Distributor is designed for *load balancing within a single site*, do not use it between sites. For information on using NServiceBus across multiple physical sites, see [the gateway](/nservicebus/gateway/multi-site-deployments.md).


## How the Distributor works

Worker nodes send messages to the distributor process, indicating when they are ready for work. These messages arrive at the distributor via a separate 'control' queue:

![worker registration](how-distributor-works-1.png)

Then the distributor creates a ready message per available thread:

![worker registration](how-distributor-works-2.png)

The distributor stores this information. When a message arrives at the distributor, it uses previously stored information to find a free worker node and sends the message to it. If no worker nodes are free, the distributor process waits before repeating the previous step.

![worker registration](how-distributor-works-3.png)

![worker registration](how-distributor-works-4.png)

All pending work stays in the distributor process' queue (rather than building up in each of the workers' queues), giving visibility of how long messages are waiting. This is important for complying with time-based service level agreements (SLAs).

For more information on monitoring, see [Performance Counters](/nservicebus/operations/metrics/performance-counters.md).

For more information about Pub/Sub in a Distributor scenario see the [Distributor and Publish-Subscribe](/transports/msmq/distributor/publish-subscribe.md) article.


## Performance

For each message being processed, the distributor process performs a few additional operations: it receives a ready message from a Worker, sends the work message to the Worker and receives a ready message post processing. That means that using Distributor introduces a certain processing overhead, that is independent of how much work is done. Therefore the Distributor is more suitable for relatively long running units of work (high I/O like http calls, writing to disk) as opposed to very short-lived units of work (a quick read from the database and dispatching a message using `Bus.Send` or  `Bus.Publish`).

To get a sense of the expected performance take maximum MSMQ throughput of a given machine (e.g. by running NServiceBus with `NOOP` handlers) and divide it by 4.

If scaling out small units of work is required consider splitting the handlers into smaller vertical slices of functionality and deploying them to dedicated endpoints.

WARNING: The default concurrency of the distributor process is set to 1. That means the messages are processed sequentially. Make sure that the [**MaximumConcurrencyLevel** has been increased in the configuration](/nservicebus/operations/tuning.md#tuning-concurrency) on the endpoint that runs distributor process. A good rule of thumb to set this value to 2-4 times the amount of cores of a given machine. While fine-tuning, inspect disk, CPU and network resources until one of these reaches its maximum capacity.

Increasing the concurrency on the workers might not lead to increased performance if the executed code is multi-threaded, e.g. if the worker does CPU-intensive work using all the available cores such as video encoding.

## High availability

If the distributor process goes down, even if its worker nodes remain running, they do not receive any messages. [It is important making sure that the distributor is running in a high-availability configuration](deploying-to-a-cluster.md).
 server migration.

## Risk on resource IO congestion

The distributor process is disk and network IO restricted. If a single endpoint does not saturate either of these, then it is possible to host multiple distributor processes on a single server.

Make sure you are monitoring your servers using infrastructure monitoring tools.

## Deployment configurations

Very often we see that first thereis a single server and when capacity isn't enough this server is mirrored/cloned with the traditional mindset to create a farm of servers and put the distributor in front as a traditional load-balancer. This often isn't the best way to scale-out and this section gives guidance on possible options with pros and cons.

In most environments there is one server that is hosting multiple endpoints. 

![Single machine configuration](configurations/single-box.png)

If one machine doesn't have enough capacity you can scale-out. Not yet by using the distributor but by moving endpoints and isolating them on their own server. if there is any risk of congestion it is evenly distributed.

![Machine per endpoint](configurations/box-per-endpoint.png)

Eventually you can reach the limits of what a single machine can do, even after scaling up. You can now selectively scale-out an endpoint. The distributor acts like a load-balancer, all messages send to *Endpoint C* will first be send to *Machine Z* and forwarded to workers if available. You will often see that there are performance issues with just a few endpoints. There is no need to scale-out all endpoints, just scale-out the endpoints that need it.

![Scale-out using distributor](configurations/distributor.png)

#### One distributor machine

This is the most common setup we encounter. There is a single distributor machine that hosts all the distributor endpoints.

![The distributor machine](configurations/single-distributor-machine.png)

This configuration has a few pros and cons

- When all endpoints are mirrored, the endpoints that are Idle most of the time which waste RAM resources.
- Deployment configuration is simple, all machines are configured the same.
- Every single message will always flow via the *Distributor machine*, this server is now a **single point of failure**. For this reason it is really important to have this configuration deployed on a high-available environment with redundant storage.
- If all machines have similar specification it would be that the workers are idle and the distributor is busy of vice versa.
- The deployment is fairly static and there is no room for tuning.
- Most simplest routing configuration as routes are pretty much the same for all servers and message mappings.


Note: This setup is often used when running on bare metal without using virtualization and common when using clustering to achieve high availability.

#### Balanced distributor setup

To mitigate most issues related to network congestion another setup can be to not have a single distributor machine but to deploy distributors across your available servers on the same servers as your workers.

![Distributor and Worker combination](configurations/distributor-worker-combo.png)

This results in the network traffic to be balanced across available servers. You now do not have a single point of failure and this model becomes more interesting with more servers especially if you have more server than you need workers.

Pros and cons:

- The distributor nodes still are not isolated and can result in congestion issues.
- No single point of failure
- You can mix and match, no need to have the same amount of workers. For some you do not need the distributor and for others you need workers on most of your servers/
- Manageable routing configuration, still capable of doing this manually.


Note: This setup

#### Single distributor per machine

![Machine per distributor](configurations/machine-per-distributor.png)

Pros and cons:

- This isolates each distributor.
- There is not a single point of failure for all your endpoints.
- Still scaling out all endpoints.
- Manageable routing configuration, still capable of doing this manually.
- Can still suffer from throughput limitations. In that case [sender side distribution](../sender-side-distribution.md) might be an alternative.

Note: This setup is more common in environments that use virtualization.


#### Machine per distributor and per worker

![Machine per distributor and per worker](configurations/machine-per-instance.png)

- Every machine works in complete isolation
- Every machine can get different specifications
- Some endpoints do not even require the distributor
- Complex routing configuration, not suitable to store routes in application configuration files.
- Can still suffer from throughput limitations. In that case [sender side distribution](../sender-side-distribution.md) might be an alternative.

Note: This setup is **very** suitable for virtualization and in IAAS cloud.
