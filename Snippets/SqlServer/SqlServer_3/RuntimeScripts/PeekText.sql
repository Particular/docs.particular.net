startcode PeekTextSql
SELECT count(*) Id FROM {0}.{1} WITH (READPAST) WHERE [Expires] IS NULL OR [Expires] > GETUTCDATE();
endcode
