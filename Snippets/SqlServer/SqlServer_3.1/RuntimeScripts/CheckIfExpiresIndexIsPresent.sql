startcode CheckIfExpiresIndexIsPresentSql
SELECT COUNT(*) FROM [sys].[indexes] WHERE [name] = '{0}' AND [object_id] = OBJECT_ID('{1}.{2}')
endcode
