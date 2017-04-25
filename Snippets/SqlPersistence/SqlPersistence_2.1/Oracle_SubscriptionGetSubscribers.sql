startcode Oracle_SubscriptionGetSubscribersSql

select distinct Subscriber, Endpoint
from "ENDPOINTNAMESS"
where MessageType in (:type0)
endcode
