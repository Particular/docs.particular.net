startcode MsSqlServer_SubscriptionSubscribeSql

merge [dbo].[EndpointNameSubscriptionData] with (holdlock) as target
using(select @Endpoint as Endpoint, @Subscriber as Subscriber, @MessageType as MessageType) as source
on target.Subscriber = source.Subscriber and
   target.MessageType = source.MessageType and
   ((target.Endpoint = source.Endpoint) or (target.Endpoint is null and source.endpoint is null))
when not matched then
insert
(
    Subscriber,
    MessageType,
    Endpoint,
    PersistenceVersion
)
values
(
    @Subscriber,
    @MessageType,
    @Endpoint,
    @PersistenceVersion
);
endcode
