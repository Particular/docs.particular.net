startcode MsSqlServer_SubscriptionUnsubscribeSql

delete from [dbo].[EndpointNameSubscriptionData]
where
    Subscriber = @Subscriber and
    MessageType = @MessageType
endcode
