startcode Oracle_TimeoutRangeSql

select Id, ExpireTime
from EndpointNameTO
where ExpireTime between :StartTime and :EndTime
endcode
