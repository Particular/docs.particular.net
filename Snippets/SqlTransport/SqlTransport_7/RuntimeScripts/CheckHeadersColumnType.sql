startcode CheckHeadersColumnTypeSql

SELECT t.name
FROM sys.columns c
INNER JOIN sys.types t ON c.system_type_id = t.system_type_id
WHERE c.object_id = OBJECT_ID('{0}')
    AND c.name = 'Headers'
endcode
