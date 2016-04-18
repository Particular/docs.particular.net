-- startcode AddSubscriber 5
Use PersistenceForMsmqTransport
Go

INSERT INTO Subscription
       ([SubscriberEndpoint]
       ,[MessageType]
       ,[Version]
       ,[TypeName])
 VALUES
       ('MsmqToSqlRelay@localhost',
       'Shared.SomethingHappened,0.0.0.0',
       '0.0.0.0',
       'Shared.SomethingHappened')
GO
-- endcode