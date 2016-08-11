--startcode sql-ExpiresIndex
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