startcode Oracle_SubscriptionGetSubscribersSql

select distinct Subscriber, Endpoint
from EndpointNameSS
where MessageType in (:type0)
endcode
