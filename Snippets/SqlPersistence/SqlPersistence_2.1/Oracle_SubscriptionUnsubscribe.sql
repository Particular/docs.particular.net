startcode Oracle_SubscriptionUnsubscribeSql

delete from "ENDPOINTNAMESS"
where
    Subscriber = :Subscriber and
    MessageType = :MessageType
endcode
