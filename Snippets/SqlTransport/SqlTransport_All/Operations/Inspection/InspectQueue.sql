startcode inspect-queue

SELECT TOP (1000) 
    [Id],
    [Expires],
    [Headers],
    [Body],
    cast([Body] as nvarchar(max)) as [BodyString]
FROM {0} WITH (READPAST)

endcode
