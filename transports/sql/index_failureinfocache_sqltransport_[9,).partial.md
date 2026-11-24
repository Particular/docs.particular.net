Each receiver keeps the failure info of up to 10,000 messages. In version 9.2 and later, this limit can be configured. When the limit is reached, the least recently used entries are removed. Lower the limit to reduce the memory held by retained exceptions, for example a `DbUpdateException` that keeps the entities tracked by its `DbContext` reachable. Keep the limit well above the endpoint's maximum concurrency. A message whose entry was removed starts again with one recorded attempt, so its retries can restart. The minimum is 3.

By default, entries are removed only when the limit is reached. Set `ExpirationPeriod` to remove the failure info of a message that hasn't failed again within that period, counted from its most recent failure. Use a period well above the longest time between two attempts of the same message.

snippet: FailureInfoCache
