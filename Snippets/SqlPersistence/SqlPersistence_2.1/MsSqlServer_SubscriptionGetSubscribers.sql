startcode MsSqlServer_SubscriptionGetSubscribersSql

select distinct Subscriber, Endpoint
from [dbo].[EndpointNameSubscriptionData]SubscriptionData
where MessageType in (@type0)
endcode
