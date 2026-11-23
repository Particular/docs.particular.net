### Chained timeouts

`AdvanceTime` handles timeouts in due time order, and sets the clock to each timeout's due time while the timeout is handled. The clock never moves backwards. A timeout requested by a timeout handler is handled in the same call when it is due before the end of the advanced time. A saga that requests a new 10-minute reminder from every reminder produces six results when advancing time by one hour:

snippet: TestableSagaChainedTimeouts

To assert on the saga between reminders, advance time in steps no longer than the reminder interval. Each result also holds a `SagaDataSnapshot` that can be inspected after a longer advance.

snippet: TestableSagaChainedTimeoutsStepwise

A timeout that is already overdue, such as one requested for an absolute time in the past, is handled at `CurrentTime` because the clock cannot move backwards. Timeouts requested by its handler are scheduled relative to that time.

`AdvanceTime` throws an `ArgumentOutOfRangeException` for a negative `TimeSpan`. It throws an `InvalidOperationException` after 1,000 timeouts are handled without the clock moving forward, which happens when handlers keep requesting timeouts that are already due, for example with a zero delay.

Each timeout is removed from storage right before it is handled. If the call is canceled or a timeout handler throws, the remaining timeouts stay stored and later calls handle them when they are due. The timeout that threw is not retried. `CurrentTime` stays at the time the last timeout that started was handled.
