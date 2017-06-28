-- startcode AddSubscriber 5
use PersistenceForMsmqTransport
go
insert into Subscription
       ([SubscriberEndpoint]
       ,[MessageType]
       ,[Version]
       ,[TypeName])
values
       ('MsmqToSqlRelay@localhost',
       'Shared.SomethingHappened,0.0.0.0',
       '0.0.0.0',
       'Shared.SomethingHappened')
go
-- endcode