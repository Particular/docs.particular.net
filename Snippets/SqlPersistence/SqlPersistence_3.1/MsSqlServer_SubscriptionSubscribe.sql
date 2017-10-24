startcode MsSqlServer_SubscriptionSubscribeSql

declare @dummy int;
merge [dbo].[EndpointNameSubscriptionData] with (holdlock, tablock) as target
using(select @Endpoint as Endpoint, @Subscriber as Subscriber, @MessageType as MessageType) as source
on target.Endpoint = source.Endpoint and
   target.Subscriber = source.Subscriber and
   target.MessageType = source.MessageType
when matched then
    update set @dummy = 0
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
