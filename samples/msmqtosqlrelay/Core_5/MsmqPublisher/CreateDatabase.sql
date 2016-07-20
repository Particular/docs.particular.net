-- startcode CreateDatabase
USE [master]
GO

/****** Object:  Database [PersistenceForMsmqTransport]    Script Date: 2/9/2016 10:20:43 AM ******/
CREATE DATABASE [PersistenceForMsmqTransport]
GO

USE [PersistenceForMsmqTransport]
GO


CREATE TABLE [dbo].[Subscription](
	[SubscriberEndpoint] [varchar](450) NOT NULL,
	[MessageType] [varchar](450) NOT NULL,
	[Version] [varchar](450) NULL,
	[TypeName] [varchar](450) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[SubscriberEndpoint] ASC,
		[MessageType] ASC
	)
)


CREATE TABLE [dbo].[TimeoutEntity](
	[Id] [uniqueidentifier] NOT NULL,
	[Destination] [nvarchar](1024) NULL,
	[SagaId] [uniqueidentifier] NULL,
	[State] [varbinary](max) NULL,
	[Time] [datetime] NULL,
	[Headers] [nvarchar](max) NULL,
	[Endpoint] [nvarchar](440) NULL,
	PRIMARY KEY NONCLUSTERED 
	(
		[Id] ASC
	)
)

GO

-- endcode