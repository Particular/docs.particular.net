-- startcode AddSubscriber
use PersistenceForMsmqTransport
go

insert into Subscription
	([SubscriberEndpoint]
	,[MessageType]
	,[LogicalEndpoint]
	,[Version]
	,[TypeName])
values
	('MsmqToSqlRelay@localhost',
	'Shared.SomethingHappened,0.0.0.0',
	'MsmqToSqlRelay' ,
	'0.0.0.0',
	'Shared.SomethingHappened')
go
-- endcode