startcode CheckIfExpiresIndexIsPresentSql

SELECT COUNT(*)
FROM sys.indexes i
INNER JOIN sys.index_columns AS ic ON ic.index_id = i.index_id AND ic.object_id = i.object_id AND ic.key_ordinal = 1
INNER JOIN sys.columns AS c ON c.column_id = ic.column_id AND c.object_id = ic.object_id
WHERE i.object_id = OBJECT_ID('{0}')
AND c.name = 'Expires'
endcode
