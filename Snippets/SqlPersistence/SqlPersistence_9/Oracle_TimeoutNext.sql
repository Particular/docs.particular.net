startcode Oracle_TimeoutNextSql

select ExpireTime
from
(
    select ExpireTime from "ENDPOINTNAMETO"
    where ExpireTime > :EndTime
    order by ExpireTime
) subquery
where rownum <= 1
endcode
