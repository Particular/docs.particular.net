---
title: Using RabbitMQ in a cluster
summary: Tips for using NServiceBus  with RabbitMQ in a cluster
component: Rabbit
reviewed: 2020-08-12
related:
 - nservicebus/operations
redirects:
 - nservicebus/rabbitmq/cluster
---

The [RabbitMQ Clustering Guide](https://www.rabbitmq.com/clustering.html#clients) recommends that client applications should not hard-code the hostnames or IP addresses of the machines hosting nodes in a RabbitMQ cluster. An approach such as DNS or load balancing should be used instead. This approach is recommended when using the NServiceBus RabbitMQ transport.

If each client is deployed to a machine which is running at least one RabbitMQ node in the cluster, another approach is to configure the client to only connect to the local node, e.g. using `localhost` as the hostname. Note that the RabbitMQ nodes must still be clustered, otherwise clients on each machine will have access only to the messages which originated locally.

partial: additionalnodes

NOTE: RabbitMQ has a concept of a single master queue and this can result in unnecessary network hops. See [load balancing a rabbitmq cluster](https://insidethecpu.com/2014/11/17/load-balancing-a-rabbitmq-cluster/).

partial: quorumqueues