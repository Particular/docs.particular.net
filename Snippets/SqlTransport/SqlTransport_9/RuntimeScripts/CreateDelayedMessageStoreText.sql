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

BEGIN TRY
    CREATE TABLE {0} (
        Headers nvarchar(max) NOT NULL,
        Body varbinary(max),
        Due datetime NOT NULL,
        RowVersion bigint IDENTITY(1,1) NOT NULL
    );

    CREATE NONCLUSTERED INDEX [Index_Due] ON {0}
    (
        [Due]
    )
END TRY
BEGIN CATCH
    EXEC sp_releaseapplock @Resource = '{0}_lock';
    THROW;
END CATCH;

EXEC sp_releaseapplock @Resource = '{0}_lock'
endcode
