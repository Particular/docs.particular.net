startcode UnsubscribeTextSql

DELETE FROM {0}
WHERE
    Endpoint = @Endpoint and
    Topic = @Topic
endcode
