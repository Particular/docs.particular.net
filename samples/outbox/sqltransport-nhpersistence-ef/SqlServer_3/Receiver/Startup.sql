if object_id('dbo.Orders') is null
begin
    create table [dbo].[Orders]
    (
        [OrderId] [nvarchar](450) not null,
        [Value] [decimal](18, 2) not null,
        constraint [PK_Orders] primary key clustered 
        (
            [OrderId] asc
        )
    );
end