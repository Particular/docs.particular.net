--startcode audit-archive

create table [dbo].[audit_archive](
	[Id] [uniqueidentifier] not null,
	[CorrelationId] [varchar](255),
	[ReplyToAddress] [varchar](255),
	[Recoverable] [bit] not null,
	[Expires] [datetime],
	[Headers] [varchar](max) not null,
	[Body] [varbinary](max),
	[RowVersion] [bigint] not null
)

--endcode