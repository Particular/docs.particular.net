startcode MsSqlServer_OutboxCleanupSql

delete top (@BatchSize) from [dbo].[EndpointNameOutboxData] with (rowlock)
where Dispatched = 'true' and
      DispatchedAt < @DispatchedBefore
endcode
