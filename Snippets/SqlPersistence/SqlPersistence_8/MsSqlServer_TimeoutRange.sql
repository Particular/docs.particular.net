startcode MsSqlServer_TimeoutRangeSql

select Id, Time
from [dbo].[EndpointNameTimeoutData]
where Time > @StartTime and Time <= @EndTime
endcode
