
In bus transports like MSMQ there is no central place from which multiple instances of an endpoint can receive messages concurrently. Each instance has its own queue so scaling out requires distributing of the messages between the queues.

The process of distributing the messages can be performed on the sender side. In this case the sender of the message knows about all the instances of the receiver. Refer to the [Scaling out with sender-side distribution](/nservicebus/msmq/sender-side-distribution.md) article for more details.

Distribution can be also done via a dedicated distributor process. Refer to [Scaling out with distributor](/nservicebus/msmq/distributor/) for details.

Following table compares both approaches:

| Sender-Side Distribution                    | Distributor                                 |
|---------------------------------------------|---------------------------------------------|
| Sender aware of scaling-out of receiver     | Sender ignorant of scaling-out of receiver  |
| Almost liner scaling with number of workers | Limited scaling                             |
| No [flow control](https://en.wikipedia.org/wiki/Flow_control_%28data%29)| [Flow control](https://en.wikipedia.org/wiki/Flow_control_%28data%29) via ready messages             |
| No automatic worker failure detection       | Worker failure detection via ready messages |
| Round-robin load balancing                  | Fair load balancing                         |


WARNING: When using sender-side distribution scaling out technique in a mixed version environment make sure to deploy a distributor in front of the scaled out version 6 endpoint if that endpoint needs to communicate with endpoints using versions lower than 6 (refer to [the distributor sample](/samples/scaleout/distributor/) for details). Otherwise event messages will get duplicated and commands won't be properly distributed.