startcode CheckIfExpiresIndexIsPresentSql

SELECT COUNT(*)
FROM sys.indexes
WHERE name = 'Index_Expires'
    AND object_id = OBJECT_ID('{0}')
endcode
