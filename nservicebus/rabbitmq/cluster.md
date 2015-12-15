---
title: Using a RabbitMQ cluster 
summary: Guidelines for using RabbitMQ in a cluster
---

The [RabbitMQ Clustering Guide](https://www.rabbitmq.com/clustering.html) recommends that you do not bake in the hostnames or IP addresses of the machines hosting nodes in a RabbitMQ cluster in your client applications and instead use an approach such DNS or load balancing. We make the same recommendation when using the NServiceBus RabbitMQ transport. For more details see the guide.

If each of your clients is deployed to a machine which is running at least one RabbitMQ node in the cluster, another approach you may consider is to configure your client to only connect to the local node, e.g. using `localhost` as the hostname. Note that the RabbitMQ nodes must still be clustered, otherwise clients on each machine will only have access to the messages which originated locally.

In Version 2.x of the RabbitMQ transport, we provided the facility to specify multiple hostnames in the connection string. We removed this facility in Version 3.0 for the reasons stated in the guide. We also found that this feature caused the following 

  - Encouraged a misconception that the RabbitMQ transport was providing load balancing by sending messages to each node in a round-robin fashion. 
  - Led to the belief that the RabbitMQ nodes themselves did not need to be clustered.

NOTE: RabbitMQ has a concept of a single master queue and this can result in unnecessary network hops. Read more [here](http://insidethecpu.com/2014/11/17/load-balancing-a-rabbitmq-cluster/).
