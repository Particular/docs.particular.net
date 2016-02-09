-- startcode CreateDatabase
USE [master]
GO

/****** Object:  Database [PersistenceForMsmqTransport]    Script Date: 2/9/2016 10:20:43 AM ******/
CREATE DATABASE [PersistenceForMsmqTransport]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PersistenceForMsmqTransport', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\PersistenceForMsmqTransport.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PersistenceForMsmqTransport_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\PersistenceForMsmqTransport_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

-- endcode