startcode PostgreSql_TimeoutRangeSql

select "Id", "Time"
from "public"."EndpointNameTimeoutData"
where "Time" > @StartTime and "Time" <= @EndTime
endcode
