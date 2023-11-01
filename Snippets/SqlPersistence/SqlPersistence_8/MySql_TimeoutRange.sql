startcode MySql_TimeoutRangeSql

select Id, Time
from `EndpointNameTimeoutData`
where Time > @StartTime and Time <= @EndTime
endcode
