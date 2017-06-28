-- startcode CreateDatabase
use [master]
go

create database [PersistenceForMsmqTransport]
go

use [PersistenceForMsmqTransport]
go

create table [dbo].[Subscription](
	[SubscriberEndpoint] [varchar](450) not null,
	[MessageType] [varchar](450) not null,
	[LogicalEndpoint] [varchar](450),
	[Version] [varchar](450),
	[TypeName] [varchar](450),
	primary key clustered
	(
		[SubscriberEndpoint],
		[MessageType]
	)
)

create table [dbo].[TimeoutEntity](
	[Id] [uniqueidentifier] not null,
	[Destination] [nvarchar](1024),
	[SagaId] [uniqueidentifier],
	[State] [varbinary](max),
	[Time] [datetime],
	[Headers] [nvarchar](max),
	[Endpoint] [nvarchar](440),
	primary key nonclustered
	(
		[Id]
	)
)

go
-- endcode