
In bus transports like MSMQ there is no central place from which multiple instances of an endpoint can receive messages concurrently. Each instance has its own queue so scaling out requires distributing of the messages between the queues. 

The [distributor](/transports/msmq/distributor/) is located between the senders and the receiving cluster. The role of the distributor is to forward incoming messages to a number of workers in order to balance the load. The workers are "invisible" to the outside world because all the outgoing messages contain the distributor's (not the worker's) address in the `reply-to` header.
