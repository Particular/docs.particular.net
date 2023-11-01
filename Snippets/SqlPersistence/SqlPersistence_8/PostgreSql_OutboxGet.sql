startcode PostgreSql_OutboxGetSql

select
    "Dispatched",
    "Operations"
from "public"."EndpointNameOutboxData"
where "MessageId" = @MessageId
endcode
