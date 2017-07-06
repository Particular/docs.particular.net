startcode CreateDelayedMessageStoreTextSql

IF EXISTS (
    SELECT * 
    FROM {1}.sys.objects 
    WHERE object_id = OBJECT_ID(N'{0}') 
        AND type in (N'U'))
RETURN

EXEC sp_getapplock @Resource = '{0}_lock', @LockMode = 'Exclusive'

IF EXISTS (
    SELECT *
    FROM {1}.sys.objects
    WHERE object_id = OBJECT_ID(N'{0}')
        AND type in (N'U'))
BEGIN
    EXEC sp_releaseapplock @Resource = '{0}_lock'
    RETURN
END

CREATE TABLE {0} (
    Headers nvarchar(max) NOT NULL,
    Body varbinary(max) NULL,
    Due datetime NOT NULL,
    RowVersion bigint IDENTITY(1,1) NOT NULL
) ON [PRIMARY];

CREATE NONCLUSTERED INDEX [Index_Due] ON {0}
(
    [Due] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
   
EXEC sp_releaseapplock @Resource = '{0}_lock'
endcode
