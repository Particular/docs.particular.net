--startcode 2to3-sqlExpiresIndex
CREATE NONCLUSTERED INDEX [Index_Expires]
ON [schema].[queuename]
(
	[Expires] ASC
)
INCLUDE
(
	[Id],
	[RowVersion]
)
--endcode