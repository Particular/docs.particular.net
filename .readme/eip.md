### EIP references

[Enterprise Integration Patterns (EIP)](https://www.enterpriseintegrationpatterns.com/) is a bible of messaging. We sometimes use the same or similar patterns, but name them differently. When describing such a pattern, it's useful to reference the related EIP pattern, to make it easier to understand.

#### Terms we use and are aligned with EIP

* Message
* Message Endpoint
* [Messaging Bridge](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessagingBridge.html) - Unfortunately we don't have any implementation other than MSMQ-SQL sample
* [Command Message](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CommandMessage.html)
* [Event Message](https://www.enterpriseintegrationpatterns.com/patterns/messaging/EventMessage.html)
* [Request-Reply](https://www.enterpriseintegrationpatterns.com/patterns/messaging/RequestReply.html) - not sure if we are 100% aligned here. We use the term _Full Duplex_ to describe a non-synchronous request reply and only use _Request-Reply_ or _Callback_ for the synchronous variant
* [Correlation ID](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CorrelationIdentifier.html)
* [Transactional Client](https://www.enterpriseintegrationpatterns.com/patterns/messaging/TransactionalClient.html)
* [Competing Consumers](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html)
* [Message Store](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageStore.html) - we have it in ServiceControl

#### EIP terms and ideas we don't use but can

* [Message Bus](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageBus.html) - we dropped using the _bus_ word when referring to an _endpoint_ (which is correct) but I think we can take advantage of this definition of the bus because it is aligned with our concepts.
* [Dead Letter Channel](https://www.enterpriseintegrationpatterns.com/patterns/messaging/DeadLetterChannel.html) - only MSMQ implements it, we don't have it on NServiceBus level
* [Datatype Channel](https://www.enterpriseintegrationpatterns.com/patterns/messaging/DatatypeChannel.html) - is a channel reserved for a single data/message type. This is something we should be selling users as a good practice
* [Channel Adapter](https://www.enterpriseintegrationpatterns.com/patterns/messaging/ChannelAdapter.html) - this seems to be similar to the ADSD idea for integration components that pull data from various services in order to combine them into a message
* [Messaging Gateway](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessagingGateway.html) and [Messaging Mapper](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessagingMapper.html) - we could prepare guidance based on them on how to use NSB in the application
* [Control Bus](https://www.enterpriseintegrationpatterns.com/patterns/messaging/ControlBus.html) - I believe we should get this implemented in (close) future
* [Test Message](https://www.enterpriseintegrationpatterns.com/patterns/messaging/TestMessage.html)

#### Terms we use but have a different meaning or name

* Message Channel - we call it a queue but EIP name seeps to be more appropriate since e.g. SQL transport does not use queues
* [Point-to-Point](https://www.enterpriseintegrationpatterns.com/patterns/messaging/PointToPointChannel.html) - we use _Unicast_ instead
* [Publish-Subscribe](https://www.enterpriseintegrationpatterns.com/patterns/messaging/PublishSubscribeChannel.html) - we use _Multicast_ instead. I think these two discrepancies are something we need to live with because we reserve _Publish/Subscribe_ name to a logical pattern.
* [Invalid Message Channel](https://www.enterpriseintegrationpatterns.com/patterns/messaging/InvalidMessageChannel.html) - we call it _error queue_
* [Guaranteed Delivery](https://www.enterpriseintegrationpatterns.com/patterns/messaging/GuaranteedMessaging.html) - we call it _store and forward_
* [Return Address](https://www.enterpriseintegrationpatterns.com/patterns/messaging/ReturnAddress.html) - we use _reply address_ but I think we are close enough.
* [Process Manager](https://www.enterpriseintegrationpatterns.com/patterns/messaging/ProcessManager.html) - we call it _Saga_
* [Message Broker](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageBroker.html) - we use the term broker to describe a centralized transport mechanism where all message channels are on remote machine/cluster. EIP sees broker as a thing that also does routing based on message types (and/or content)
* [Claim Check](https://www.enterpriseintegrationpatterns.com/patterns/messaging/StoreInLibrary.html) - we call it _Data bus_
* [Event-Driven Consumer](https://www.enterpriseintegrationpatterns.com/patterns/messaging/EventDrivenConsumer.html) - we call it _message pump_ or _IPushMessages_
* [Message Dispatcher](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageDispatcher.html) - we call it _Distributor_
* [Selective Consumer](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageSelector.html) - I believe our closest equivalent is a message handler
* [Wire Tap](https://www.enterpriseintegrationpatterns.com/patterns/messaging/ControlBus.html) - we call it audit queue which confuses the purpose with the implementation. The thing is called _wire tap_ and it is usually used to _audit_ message flows.