startcode CreateQueueTextSql
IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'U'))
                  BEGIN
                    EXEC sp_getapplock @Resource = '{0}_{1}_lock', @LockMode = 'Exclusive'

                    IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE [{0}].[{1}](
                            [Id] [uniqueidentifier] NOT NULL,
                            [CorrelationId] [varchar](255) NULL,
                            [ReplyToAddress] [varchar](255) NULL,
                            [Recoverable] [bit] NOT NULL,
                            [Expires] [datetime] NULL,
                            [Headers] [varchar](max) NOT NULL,
                            [Body] [varbinary](max) NULL,
                            [RowVersion] [bigint] IDENTITY(1,1) NOT NULL
                        ) ON [PRIMARY];

                        CREATE CLUSTERED INDEX [Index_RowVersion] ON [{0}].[{1}]
                        (
                            [RowVersion] ASC
                        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

                        CREATE NONCLUSTERED INDEX [Index_Expires] ON [{0}].[{1}]
                        (
                            [Expires] ASC
                        )
                        INCLUDE
                        (
                            [Id],
                            [RowVersion]
                        )
                        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
                    END

                    EXEC sp_releaseapplock @Resource = '{0}_{1}_lock'
                  END
endcode
