## MARS

All [SqlConnection](https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.aspx)s must have [Multiple Active Result Sets (MARS)
](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/multiple-active-result-sets-mars) as multiple concurrent async request can be performed.