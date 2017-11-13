startcode PostgreSql_TimeoutNextSql

select "Time" from "public"."EndpointNameTimeoutData"
where "Time" > @EndTime
order by "Time"
limit 1
endcode
