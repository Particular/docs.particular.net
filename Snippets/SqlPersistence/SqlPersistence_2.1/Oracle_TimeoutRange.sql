startcode Oracle_TimeoutRangeSql

select Id, ExpireTime
from "ENDPOINTNAMETO"
where ExpireTime between :StartTime and :EndTime
endcode
