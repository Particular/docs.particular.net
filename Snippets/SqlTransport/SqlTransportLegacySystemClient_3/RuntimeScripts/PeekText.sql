startcode PeekTextSql

SELECT count(*) Id
FROM (
    SELECT TOP {1} * 
    FROM {0} WITH (READPAST) 
    WHERE Expires IS NULL OR Expires > GETUTCDATE()
) as count_table;
endcode
