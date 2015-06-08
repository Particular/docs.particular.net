---
title: Using a RabbitMQ cluster 
summary: Guidelines for using RabbitMQ in a cluster
---

The [RabbitMQ Clustering Guide](https://www.rabbitmq.com/clustering.html) recommends that you do not bake in the hostnames or IP addresses of nodes in a RabbitMQ cluster in your client applications and instead use an approach such DNS or load balancing. We make the same recommendation when using the NServiceBus RabbitMQ transport. For more details see the guide.

If each of your clients is deployed to a machine which is running at least one RabbitMQ host from the cluster, another approach you may consider is to configure your client to only connect to the local RabbitMQ instance, i.e. using `localhost` as the hostname. Note that the RabbitMQ hosts must still be clustered, otherwise each machine will only have access to the messages which originated locally.

In version 2.x of the Rabbit MQ transport, we provided the facility to specify multiple hosts in the connection string. We removed this facility in version 3.0 for the reasons stated in the guide. We also found that this feature a) encouraged a misconception that the RabbitMQ transport was providing load balancing by sending messages to each host in a round-robin fashion and b) led to the belief that the RabbitMQ hosts themselves did not need to be clustered.
