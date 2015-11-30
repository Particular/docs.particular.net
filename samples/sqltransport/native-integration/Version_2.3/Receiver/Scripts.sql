
-- startcode SendFromTSQL
-- TSql that you can paste into Sql Server Query analyzer to send straight from the DB
INSERT INTO [Samples.SqlServer.NativeIntegration] ([Id],[Recoverable],[Headers],[Body]) 
VALUES	(CONVERT(UNIQUEIDENTIFIER, HASHBYTES('MD5','MyUniqueId')),
		'true',
		'',
		CONVERT(varbinary(255), '{ $type: "PlaceOrder",OrderId: "Order from TSQL sender"}'))
-- endcode

-- startcode CreateLegacyTable
-- Create a "legacy" Orders table
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderValue] [money] NOT NULL
) ON [PRIMARY]
GO
-- endcode


-- startcode CreateTrigger
-- Create a trigger to push a message out for each new order
CREATE TRIGGER [dbo].[OrderAcceptedTrigger] 
	ON  [dbo].[Orders]
	AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Samples.SqlServer.NativeIntegration] ([Id],[Recoverable],[Headers],[Body]) 
	SELECT CONVERT(UNIQUEIDENTIFIER, HASHBYTES('MD5',CONVERT(VARCHAR(255),i.Id))) as Id,
	'true' as Recoverable,
	'' as Headers,
	CONVERT(varbinary(255), '{ $type: "LegacyOrderDetected",OrderId: "Hello from legacy Order ' + CONVERT(VARCHAR(255),i.Id) + '"}')as Body
	FROM inserted i
END
GO
-- endcode




