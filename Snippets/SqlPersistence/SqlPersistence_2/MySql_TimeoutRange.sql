startcode MySql_TimeoutRangeSql

select Id, Time
from `EndpointNameTimeoutData`
where Time between @StartTime and @EndTime
endcode
