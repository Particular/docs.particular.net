--startcode sql-2.2.2-ExpiresIndex [2.2.2,)
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