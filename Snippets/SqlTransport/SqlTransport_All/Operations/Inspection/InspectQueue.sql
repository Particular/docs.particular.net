startcode inspect-queue

SELECT TOP (1000) 
    [Id],
    [Expires],
    [Headers],
    [Body],
    cast([Body] as varchar(max)) as [BodyString]
FROM {0} WITH (READPAST)

endcode

startcode inspect-queue-computedColumn

SELECT TOP (1000) 
    [Id],
    [Expires],
    [Headers],
    [Body],
    [BodyString]
FROM {0} WITH (READPAST)

endcode

