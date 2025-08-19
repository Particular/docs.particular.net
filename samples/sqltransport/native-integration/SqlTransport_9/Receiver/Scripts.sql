
-- startcode SendFromTSQL
-- TSql that can be pasted into Sql Server Query analyzer to send straight from the DB
insert into [Samples.SqlServer.NativeIntegration]
			(Id , Recoverable, Headers, Body)
values (convert(uniqueidentifier, hashbytes('MD5','MyUniqueId')),
    'true',
    '',
    convert(varbinary(255), '{ $type: "PlaceOrder", OrderId: "Order from TSQL sender"}'))
-- endcode

-- startcode CreateLegacyTable
-- Create a "legacy" Orders table
create table Orders(
  Id int identity(1,1) not null,
  OrderValue money not null
)
go
-- endcode


-- startcode CreateTrigger
-- Create a trigger to push a message out for each new order
create trigger OrderAcceptedTrigger
  ON Orders
  after insert
as
begin
  set nocount on;

  insert into [Samples.SqlServer.NativeIntegration]
			  (Id, Recoverable, Headers, Body)
  select convert(uniqueidentifier, hashbytes('MD5',convert(varchar(255),i.Id))) as Id,
  'true' as Recoverable,
  '' as Headers,
  convert(varbinary(255), '{ $type: "LegacyOrderDetected", OrderId: "Hello from legacy Order ' + convert(varchar(255),i.Id) + '"}') as Body
  from inserted i
end
go
-- endcode