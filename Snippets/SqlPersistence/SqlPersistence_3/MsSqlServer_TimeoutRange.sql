startcode MsSqlServer_TimeoutRangeSql

select Id, Time
from [dbo].[EndpointNameTimeoutData]
where Time between @StartTime and @EndTime
endcode
