startcode MySql_TimeoutNextSql

select Time from `EndpointNameTimeoutData`
where Time > @EndTime
order by Time
limit 1
endcode
