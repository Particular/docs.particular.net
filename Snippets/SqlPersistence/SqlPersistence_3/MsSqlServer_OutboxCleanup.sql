startcode MsSqlServer_OutboxCleanupSql

delete top (@BatchSize) from [dbo].[EndpointNameOutboxData]
where Dispatched = 'true' and
      DispatchedAt < @DispatchedBefore
endcode
