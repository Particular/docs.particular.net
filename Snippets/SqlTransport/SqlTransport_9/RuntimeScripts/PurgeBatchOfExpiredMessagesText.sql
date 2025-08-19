startcode PurgeBatchOfExpiredMessagesTextSql

DELETE FROM {0}
WHERE RowVersion
    IN (SELECT TOP (@BatchSize) RowVersion
        FROM {0} WITH (READPAST)
        WHERE Expires < GETUTCDATE())
endcode
