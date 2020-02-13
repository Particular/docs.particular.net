startcode GetSubscribersTextSql

SELECT DISTINCT QueueAddress
FROM {0}
WHERE Topic IN ({1})

endcode
