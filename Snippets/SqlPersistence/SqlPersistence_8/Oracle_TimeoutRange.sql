startcode Oracle_TimeoutRangeSql

select Id, ExpireTime
from "ENDPOINTNAMETO"
where ExpireTime > :StartTime and ExpireTime <= :EndTime
endcode
