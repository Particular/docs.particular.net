startcode PurgeBatchOfExpiredMessagesTextSql
DELETE FROM {1}.{2} WHERE [Id] IN (SELECT TOP ({0}) [Id] FROM {1}.{2} WITH (UPDLOCK, READPAST, ROWLOCK) WHERE [Expires] < GETUTCDATE() ORDER BY [RowVersion])
endcode
