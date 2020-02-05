startcode MySql_SubscriptionGetSubscribersSql

select distinct Subscriber, Endpoint
from `EndpointNameSubscriptionData`
where MessageType in (@type0)
endcode
