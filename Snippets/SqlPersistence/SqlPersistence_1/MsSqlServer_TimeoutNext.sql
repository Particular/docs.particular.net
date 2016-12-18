startcode MsSqlServer_TimeoutNextSql

select top 1 Time from EndpointNameTimeoutData
where Time > @EndTime
order by Time
endcode
