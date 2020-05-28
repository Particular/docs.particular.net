---
title: Why Use SQS?
summary: Explore when it makes sense to use SQS as a transport.
component: SQS
reviewed: 2019-07-15
redirects:
 - nservicebus/sqs/why-use-sqs
---

When building a system using NServiceBus that is deployed on auto-scaled EC2 instances, using SQS as a transport provides the least amount of friction in terms of development and operations.

## Importance of Auto Scaling

Auto Scaling (AS) matters because it minimizes infrastructure costs and gives the system self-healing capabilities.

Auto Scaling exists so that a system can scale up and down based on demand. When the demands on a system are high, e.g. there are many incoming web requests or many messages in a queue, auto scaling should automatically add more workers to keep up. When that demand subsides, auto scaling should automatically terminate workers to minimize unused infrastructure and keep costs to a minimum. If configured well, AS will give a system just enough processing power to cope with the load on it at any given time, for the lowest possible cost.

Auto Scaling isn't just about scaling however. Perhaps even more importantly, it gives a system the ability to recover from faults automatically - to heal itself.

Auto Scaling periodically performs a customizable health check on each server in the group. If a server is "unhealthy", it is terminated. A brand new instance is then created to replace it. This mechanism gives a system the capability to automatically recover from most faults that occur in the servers.

To put it simply, systems built on AWS that don't use AS may cost more to run, and they may not be as resilient as they could be.


## Comparing Infrastructure Costs With Other Message Brokers on AWS

Deploying a message broker (e.g., RabbitMQ) on EC2 is a viable alternative to SQS. Comparing the costs of SQS with other message brokers deployed on EC2 is not straightforward: SQS is billed per API call whereas the EC2 instances running the message broker are billed per hour.

To compare costs fairly, the message broker should have similar availability and durability characteristics as SQS. A hypothetical message broker for comparison purposes should be run on at least two EC2 instances, running 24*7, in separate Availability Zones, each with an EBS volume for durable storage of messages. The cost of running these instances can be calculated from the [AWS Pricing Calculator](https://calculator.s3.amazonaws.com/index.html). For example, consider two t2.micro instances that cost around $30 per month.

The cost of running the message broker can be compared to the how many SQS API calls can be made for the same cost. Continuing the above example, spending $30 per month on SQS allows for 75 million API calls per month. The throughput of the hypothetical message broker can then be measured to see if it can handle the same load as SQS with the same cost (for example, 75 million API calls per month).

Very broadly speaking, SQS is cheaper to run when compared to other message brokers on EC2 until the volume of messages approaches millions per hour.

Note that this comparison only takes into account the dollar cost paid to AWS, and does not factor in the time and effort required to set up, maintain, secure and monitor a message broker on EC2.

## AWS Auto-Scaling Doesn't Play Nice With Local MSMQ

Consider the following idiomatic NServiceBus setup on AWS. An endpoint is deployed on an EC2 instance. The EC2 instance has an EBS volume attached. The endpoint uses local MSMQ's that are stored on the EBS volume.

This approach gives great sending performance (as sending only needs to write to the local drive) and great durability of messages (as EBS is geo-redundant by default). This approach will work really well for a ["pet"](https://www.lauradhamilton.com/servers-pets-versus-cattle) EC2 instance.

However, this approach doesn't mesh well with [AWS Auto Scaling](https://aws.amazon.com/autoscaling/). When AS needs to scale down, it simply terminates EC2 instances with little warning. AS provides no mechanism to allow any on-instance queues to drain before the instance is terminated. It is possible to hook in to the shutdown flow offered by EC2 and start offloading queue messages when a shutdown is imminent, but it can never be guaranteed that messages won't be lost - once the shutdown time arrives, it's lights out, regardless if there are messages still in the queues or not.  If the default AS setup is used, the EBS volume attached to the instance will be terminated as well - resulting in loss of messages.

AS can be configured to not terminate any EBS volumes when scaling down. However this leaves an orphaned volume, potentially with messages on it, with nothing processing them. Naturally this is a problem when there are SLAs to meet.

To solve these problems, the guidance from Amazon is to use SQS as the transport. This transport will help get the most out of AWS and the Auto Scaling feature.