
In bus transports like MSMQ there is no central place from which multiple instances of an endpoint can receive messages concurrently. Each instance has its own queue so scaling out requires distributing of the messages between the queues.

The process of distributing the messages can be performed on the sender side. In this case the sender of the message knows about all the instances of the receiver. Refer to the [Scaling out with sender-side distribution](/transports/msmq/sender-side-distribution.md) article for more details.

Distribution can be also done via a dedicated distributor process. Refer to [Scaling out with distributor](/transports/msmq/distributor/) for details.

Following table compares both approaches:

| Sender-Side Distribution                    | Distributor                                 |
|---------------------------------------------|---------------------------------------------|
| Sender aware of scaling-out of receiver     | Sender ignorant of scaling-out of receiver  |
| Almost liner scaling with number of workers | Limited scaling                             |
| No [flow control](https://en.wikipedia.org/wiki/Flow_control_%28data%29)| [Flow control](https://en.wikipedia.org/wiki/Flow_control_%28data%29) via ready messages             |
| No automatic worker failure detection       | Worker failure detection via ready messages |
| Round-robin load balancing                  | Fair load balancing                         |


WARNING: A scaled-out endpoint without a Distributor cannot subscribe to events published by an endpoint running Version 5 or lower of NServiceBus, otherwise each event will be delivered to each instance. The workaround is to put a Distributor in front of the scaled-out endpoint. Refer to [the distributor sample](/samples/scaleout/distributor/) for details).

include: sender-side-distribution-with-distributor
