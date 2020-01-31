startcode MySql_SubscriptionUnsubscribeSql

delete from `EndpointNameSubscriptionData`
where
    Subscriber = @Subscriber and
    MessageType = @MessageType
endcode
