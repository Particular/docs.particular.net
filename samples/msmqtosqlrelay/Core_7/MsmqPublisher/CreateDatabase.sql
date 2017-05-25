-- startcode CreateDatabase
USE [master]
GO

CREATE DATABASE [PersistenceForMsmqTransport]
GO

USE [PersistenceForMsmqTransport]
GO

CREATE TABLE [dbo].[Subscription](
	[SubscriberEndpoint] [varchar](450) NOT NULL,
	[MessageType] [varchar](450) NOT NULL,
	[LogicalEndpoint] [varchar](450),
	[Version] [varchar](450),
	[TypeName] [varchar](450),
	PRIMARY KEY CLUSTERED 
	(
		[SubscriberEndpoint] ASC,
		[MessageType] ASC
	)
)

CREATE TABLE [dbo].[TimeoutEntity](
	[Id] [uniqueidentifier] NOT NULL,
	[Destination] [nvarchar](1024),
	[SagaId] [uniqueidentifier],
	[State] [varbinary](max),
	[Time] [datetime],
	[Headers] [nvarchar](max),
	[Endpoint] [nvarchar](440),
	PRIMARY KEY NONCLUSTERED 
	(
		[Id] ASC
	)
)

GO
-- endcode