startcode PostgreSql_SubscriptionGetSubscribersSql

select distinct "Subscriber", "Endpoint"
from "public"."EndpointNameSubscriptionData"
where "MessageType" in (@type0)
endcode
