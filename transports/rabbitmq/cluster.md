---
title: In a cluster
component: Rabbit
reviewed: 2017-01-13
related:
 - nservicebus/operations
redirects:
 - nservicebus/rabbitmq/cluster
---

The [RabbitMQ Clustering Guide](https://www.rabbitmq.com/clustering.html) recommends that client applications should not hard code the hostnames or IP addresses of the machines hosting nodes in a RabbitMQ cluster. An approach such as DNS or load balancing should be used instead. This approach is recommended when using the NServiceBus RabbitMQ transport.

If each client is deployed to a machine which is running at least one RabbitMQ node in the cluster, another approach is to configure the client to only connect to the local node, e.g. using `localhost` as the hostname. Note that the RabbitMQ nodes must still be clustered, otherwise clients on each machine will only have access to the messages which originated locally.

In Version 2.x of the RabbitMQ transport, multiple hostnames could be specified in the connection string. This capability was removed in Version 3.0 for the reasons stated in the Clustering Guide. This feature caused the following issues:

 * Encouraged a misconception that the RabbitMQ transport was providing load balancing by sending messages to each node in a round-robin fashion.
 * Led to the belief that the RabbitMQ nodes themselves did not need to be clustered.

NOTE: RabbitMQ has a concept of a single master queue and this can result in unnecessary network hops. See [load balancing a rabbitmq cluster](https://insidethecpu.com/2014/11/17/load-balancing-a-rabbitmq-cluster/).
