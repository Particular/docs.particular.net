startcode MsSqlServer_SubscriptionSubscribeSql

declare @dummy int;
merge [dbo].[EndpointNameSubscriptionData] with (holdlock, tablock) as target
using(select @Endpoint as Endpoint, @Subscriber as Subscriber, @MessageType as MessageType) as source
on target.Subscriber = source.Subscriber 
    and target.MessageType = source.MessageType
when matched and source.Endpoint is not null and (target.Endpoint is null or target.Endpoint <> source.Endpoint) then
update set Endpoint = @Endpoint, PersistenceVersion = @PersistenceVersion
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
