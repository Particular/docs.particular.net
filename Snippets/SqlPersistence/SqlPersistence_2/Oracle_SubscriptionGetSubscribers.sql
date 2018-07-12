startcode Oracle_SubscriptionGetSubscribersSql

select distinct Subscriber, Endpoint
from "dbo"."ENDPOINTNAMESS"
where MessageType in (:type0)
endcode
