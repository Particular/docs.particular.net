startcode PostgreSql_SubscriptionUnsubscribeSql

delete from "public"."EndpointNameSubscriptionData"
where
    "Subscriber" = @Subscriber and
    "MessageType" = @MessageType
endcode
