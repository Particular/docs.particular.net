startcode Oracle_TimeoutRangeSql

select Id, ExpireTime
from "dbo"."ENDPOINTNAMETO"
where ExpireTime > :StartTime and ExpireTime <= :EndTime
endcode
