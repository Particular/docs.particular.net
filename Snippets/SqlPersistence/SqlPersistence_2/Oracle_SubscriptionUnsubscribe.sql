startcode Oracle_SubscriptionUnsubscribeSql

delete from EndpointNameSS
where
    Subscriber = :Subscriber and
    MessageType = :MessageType
endcode
