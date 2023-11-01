startcode MoveDueDelayedMessageTextSql

;WITH message AS (
    SELECT TOP(@BatchSize) *
    FROM {0} WITH (UPDLOCK, READPAST, ROWLOCK)
    WHERE Due < GETUTCDATE())
DELETE FROM message
OUTPUT
    NEWID(),
    NULL,
    NULL,
    1,
    NULL,
    deleted.Headers,
    deleted.Body
INTO {1};

SELECT TOP 1 GETUTCDATE() as UtcNow, Due as NextDue
FROM {0}
ORDER BY Due";
endcode
