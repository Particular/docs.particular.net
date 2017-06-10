---
title: Why Use SQS As Your Transport?
summary: Explore when it makes sense to use SQS as a transport.
versions: '[1,)'
tags:
- AWS
---

If you're running a system using NServiceBus that is deployed on auto-scaled EC2 instances, using SQS gives you the least amount of friction in terms of development and operations.

## AWS Auto-Scaling Doesn't Play Nice With Local MSMQ

Consider the following idiomatic NServiceBus setup on AWS. An endpoint is deployed on an EC2 instance. The EC2 instance has an EBS volume attached. The endpoint uses local MSMQ's that are stored on the EBS volume. 

This approach gives you great sending performance (as Bus.Send only needs to write to the local drive) and great durability of your messages (as EBS is geo-redundant by default). This approach will work really well for a ["pet"](http://www.lauradhamilton.com/servers-pets-versus-cattle) EC2 instance.

However, this approach doesn't mesh well with [AWS Auto Scaling](http://aws.amazon.com/autoscaling/) (AS hereafter). When AS needs to scale down, it simply terminates EC2 instances with little warning. AS provides no mechanism to allow any on-instance queues to drain before the instance is terminated. You can try to hook in to the shutdown flow offered by EC2, and start offloading queue messages when a shutdown is imminent, but you can never guarantee that you won't lose messages - once the shutdown time arrives, it's lights out, regardless if there are messages still in your queues or not.  If you're using the default AS setup, the EBS volume attached to the instance will be terminated as well - resulting in loss of messages.

You could configure AS to not terminate any EBS volumes when scaling down - however, this leaves you with an orphaned volume, potentially with messages on it, with nothing processing them. Naturally this is a problem if you have SLAs to meet.

You could certainly invest some time and effort into coming up with solutions to these problems. However, the guidance from Amazon is, very simply, "use SQS as your queue!" Using SQS as your transport will help you get the most out of AWS and the Auto Scaling feature in particular.

## Hang On, Why Should I Care About Auto Scaling?

Because it minimizes your infrastructure costs and gives your system self-healing capabilities.

Auto-scaling exists so you can scale up and down based on demand. When the demands on your system are high, e.g., there are many incoming web requests or many messages in a queue, auto scaling should automatically add more workers to keep up, and when that demand subsides, auto scaling should automatically terminate workers so that you aren't paying for unused infrastructure. If configured well, Auto Scaling will give you just enough processing power to cope with the load on your system at any given time, for the lowest possible cost.

Auto-scaling isn't just about scaling however. Perhaps even more importantly, it gives your system the ability to recover from faults automatically - to heal itself.

Auto-scaling periodically performs a customisable health check on each server in the group. If a server is "unhealthy", it is simply terminated. A brand new instance is then created to replace it. This mechanism gives your system the capability to automatically recover from pretty much any kind of fault that occurs in your servers.

To put it simply, if you're on AWS, and you're not using Auto Scaling, you might be paying too much for EC2, and your system may not be as resilient as it could be.

## What About RabbitMQ?

The short answer - RabbitMQ is a great technology. If you're expecting a very high volume of messages (i.e., millions per hour), it will cost you orders of magnitude less money than SQS. For systems expecting smaller volumes of messages, RabbitMQ will cost you roughly the same as SQS. 

Regardless of the size of the system, RabbitMQ will cost you a lot more time and effort to set up, maintain and monitor when compared SQS. 

### Infrastructure Costs

You could have a single EC2 instance that serves as your RabbitMQ broker, and chances are this setup will be a lot cheaper than SQS. However, this setup isn't really a fair comparison, as it's not highly available like SQS is. It also won't be as durable as SQS unless we add an EBS volume. So, in the interests of a fair comparison, let's consider the smallest possible RabbitMQ deployment that has similar availability and durability characteristics as SQS. We can then fairly compare the costs of both technologies.

Let's consider two t2.micro instances. Each one hosts a RabbitMQ cluster node - they both form a single logical broker. Each one resides in a different AZ, so if one AZ goes down, the broker will still be available. Each one has a 30GB EBS volume that will be used to persist messages, so the messages won't be lost if one of the instances is terminated. This setup gives us comparable availability and durability characteristics to SQS.

This setup will cost a minimum of $23.40 per month (using reserved instances for a 3 year term). This is the cost for a single broker; if you want to have additional brokers for development, QA, staging / pre-production, etc., you'll need to pay more. Note that this is the money you pay to Amazon, and doesn't count the cost of your time for setting this up.

Now let's consider a budget of $23.40 per month if we were to spend it on SQS. $23.40 gives us 47.8 million API calls for the month. Let's say we do 10 million receives, and each receive call receives 3 messages on average. Say we do 30 million sends, and the other 7.8 million are "other" API calls (e.g., creating, purging, polling empty queues). That's 30 million messages a month, or a million messages per day, or about 11 messages per second. More importantly, it takes significantly less time to set this up. 

Can the RabbitMQ cluster of t2.micro instances send and receive 11 durable messages per second? I'm not going to try to answer that properly, but based on the many benchmarks that are available out there I'll guess that it can probably do *more* than that. It could definitely do a *lot* more if you turned off persistence - which isn't an option in SQS. (I'm not going to bother measuring this properly, because performance isn't really my point here at all. Let's just say RabbitMQ is same or better and leave it at that). 

Bottom line - spending the same amount of money (but significantly different amounts of *time*), you'll get roughly the same or better performance out of RabbitMQ.

So why wouldn't you use RabbitMQ?

### Maintenance - The Most Expensive Phase Of Any Project

As I've alluded to already, RabbitMQ requires significant investment to set up, maintain and monitor. Remember, with a broker style architecture, the broker is a single point of failure - so you have to spend a lot of time bulletproofing it. You'll need to set it up with high availability and durability, which means clustering or federation. You'll have to integrate RabbitMQ with your monitoring solution, and then put all the processes in place to monitor the broker in production. You'll need to detect when hard drives are nearly full, when CPU or IO is nearly saturated, and take appropriate action.  You may want to consider securing your queues with the LDAP plugin, and you may want to integrate that with your monitoring as well, so you can be notified of many authentication failures. You'll probably spend a chunk of time doing performance tuning, and you'll spend time updating the software as new RabbitMQ versions are released. 

Naturally, with certain systems or certain teams, all that work will be well and truly worth it. With others, it may seem like overkill.

With SQS, you don't need to worry about any of that. Production quality security (with IAM), availability, durability, maintenance and updates are all done for you, transparently. Just sign up an AWS account, install and configure NServiceBus.AmazonSQS, and you're ready to go. Amazon take care of maintaining the queueing infrastructure for you. Over the course of a product with a lifetime of more than a few years, this could amount to a huge *overall* cost saving.