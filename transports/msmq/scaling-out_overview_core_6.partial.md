
In bus transports like MSMQ there is no central place from which multiple instances of an endpoint can receive messages concurrently. Each instance has its own queue so scaling out requires distributing messages between the queues.

The process of distributing the messages can be performed on the sender. In this case, the sender of the message knows about all the instances of the receiver. Refer to [Scaling Out with Sender-side Distribution](/transports/msmq/sender-side-distribution.md) for more details.

Distribution can also be done via a dedicated distributor process. Refer to [Scaling Out with the Distributor](/transports/msmq/distributor/) for details.

The following table compares each approach:

| Sender-Side Distribution                     | Distributor                                 |
|----------------------------------------------|---------------------------------------------|
| Sender aware of scaled-out receiver         | Sender ignorant of scaled-out receiver      |
| Almost linear scaling with number of workers | Limited scaling                             |
| No [flow control](https://en.wikipedia.org/wiki/Flow_control_%28data%29)| [Flow control](https://en.wikipedia.org/wiki/Flow_control_%28data%29) via ready messages             |
| No automatic worker failure detection       | Worker failure detection via ready messages |
| Round-robin load balancing                  | Fair load balancing                         |
| Available since core v6 & msmq v1           | Unsupported since core v7 / msmq v1         |



WARNING: A scaled-out endpoint without a Distributor cannot subscribe to events published by an endpoint running Version 5 or lower of NServiceBus, otherwise each event will be delivered to each instance.

include: sender-side-distribution-with-distributor
