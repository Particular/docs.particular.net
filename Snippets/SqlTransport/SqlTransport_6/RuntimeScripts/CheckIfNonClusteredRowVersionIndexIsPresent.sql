startcode CheckIfNonClusteredRowVersionIndexIsPresentSql

SELECT COUNT(*)
FROM sys.indexes
WHERE name = 'Index_RowVersion'
    AND object_id = OBJECT_ID('{0}')
    AND type = 2
endcode
