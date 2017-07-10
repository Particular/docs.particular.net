--startcode sql-ExpiresIndex
create nonclustered index [Index_Expires]
on [schema].[queuename]
(
    [Expires] asc
)
include
(
    [Id],
    [RowVersion]
)
--endcode