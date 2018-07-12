startcode Oracle_SubscriptionUnsubscribeSql

delete from "dbo"."ENDPOINTNAMESS"
where
    Subscriber = :Subscriber and
    MessageType = :MessageType
endcode
