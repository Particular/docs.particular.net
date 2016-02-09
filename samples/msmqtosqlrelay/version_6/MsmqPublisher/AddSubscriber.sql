
-- startcode AddSubscriber 6
Use PersistenceForMsmqTransport
Go

INSERT INTO Subscription 
		([SubscriberEndpoint] 
		,[MessageType] 
		,[LogicalEndpoint] 
		,[Version] 
		,[TypeName])
     VALUES
        ('MsmqToSqlRelay@localhost', 
		'Shared.SomethingHappened,0.0.0.0', 
		'MsmqToSqlRelay' ,
		'0.0.0.0', 
		'Shared.SomethingHappened')
GO
-- endcode