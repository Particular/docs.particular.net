startcode MySql_SubscriptionGetSubscribersSql

select distinct Subscriber, Endpoint
from `EndpointNameSubscriptionData`SubscriptionData
where MessageType in (@type0)
endcode
