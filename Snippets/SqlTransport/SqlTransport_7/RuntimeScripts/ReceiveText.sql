startcode ReceiveTextSql

DECLARE @NOCOUNT VARCHAR(3) = 'OFF';
IF ( (512 & @@OPTIONS) = 512 ) SET @NOCOUNT = 'ON';
SET NOCOUNT ON;

WITH message AS (
    SELECT TOP(1) *
    FROM {0} WITH (UPDLOCK, READPAST, ROWLOCK)
    ORDER BY RowVersion)
DELETE FROM message
OUTPUT
    deleted.Id,
    deleted.CorrelationId,
    deleted.ReplyToAddress,
    CASE WHEN deleted.Expires IS NULL
        THEN 0
        ELSE CASE WHEN deleted.Expires > GETUTCDATE()
            THEN 0
            ELSE 1
        END
    END,
    deleted.Headers,
    deleted.Body;

IF (@NOCOUNT = 'ON') SET NOCOUNT ON;
IF (@NOCOUNT = 'OFF') SET NOCOUNT OFF;
endcode
