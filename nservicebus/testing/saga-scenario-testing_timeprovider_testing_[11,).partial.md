## Using a time provider

A saga that reads the current time should take a `TimeProvider` as a dependency instead of calling `DateTime.UtcNow`. `TestableSaga` is backed by a [`FakeTimeProvider`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.time.testing.faketimeprovider) from the `Microsoft.Extensions.TimeProvider.Testing` package, so the time the saga reads is the time that `AdvanceTime` moves. Timers that other code creates on the same provider fire in order as time advances.

The factory overload receives the `FakeTimeProvider` that the testable saga creates. It starts at `DateTimeOffset.UtcNow`:

snippet: TestableSagaCtorTimeProvider

To share the clock with other test doubles or to start at a specific time, create the `FakeTimeProvider` in the test and pass it to the constructor:

snippet: TestableSagaCtorSharedTimeProvider

The provider is available as `TestableSaga.TimeProvider`, and `CurrentTime` reads from it. While `AdvanceTime` handles a timeout, the provider reports the due time of that timeout. When all timeouts are handled, the provider is at the requested end time:

snippet: TestableSagaTimeProviderDueTime

Time advanced directly on the provider is also honored. New timeouts are scheduled relative to it, and timeouts that become due are handled by the next call to `AdvanceTime`, for example `AdvanceTime(TimeSpan.Zero)`:

snippet: TestableSagaTimeProviderDirect
