startcode AddMessageBodyStringColumnSql

IF NOT EXISTS (
    SELECT *
    FROM {1}.sys.objects
    WHERE object_id = OBJECT_ID(N'{0}')
        AND type in (N'U'))
RETURN

IF EXISTS (
  SELECT * 
  FROM   {1}.sys.columns 
  WHERE  object_id = OBJECT_ID(N'{0}') 
         AND name = 'BodyString'
)
RETURN

EXEC sp_getapplock @Resource = '{0}_lock', @LockMode = 'Exclusive'

IF EXISTS (
  SELECT * 
  FROM   {1}.sys.columns 
  WHERE  object_id = OBJECT_ID(N'{0}') 
         AND name = 'BodyString'
)
BEGIN
    EXEC sp_releaseapplock @Resource = '{0}_lock'
    RETURN
END

ALTER TABLE {0} 
ADD BodyString as cast(Body as nvarchar(max));

EXEC sp_releaseapplock @Resource = '{0}_lock'
endcode
