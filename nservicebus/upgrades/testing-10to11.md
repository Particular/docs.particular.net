---
title: NServiceBus Testing Upgrade Version 10 to 11
summary: Instructions on how to upgrade NServiceBus.Testing Version 10 to 11.
reviewed: 2026-10-30
component: Testing
related:
 - nservicebus/testing
 - nservicebus/testing/saga-scenario-testing
isUpgradeGuide: true
---

NServiceBus.Testing version 11 changes how `TestableSaga` handles time. The changes can make existing [saga scenario tests](/nservicebus/testing/saga-scenario-testing.md) fail even though the saga itself is correct. Handler, behavior, and message session tests are not affected.

## Scenario impact summary

| Scenario | Affected | What to do |
| --- | --- | --- |
| Tests that chain timeouts, such as a recurring reminder saga, and assert the number of results of each `AdvanceTime` call | Yes | Update the expected counts, or [advance time in smaller steps](#chained-timeouts-are-handled-in-the-same-advancetime-call) |
| Tests that only check whether a timeout fires and do not chain timeouts | No | Nothing |
| Tests with timeouts requested for a time in the past | Yes | Expect the timeout to be handled at `CurrentTime`, and follow-up timeouts to count from then |
| Tests that pass a negative `TimeSpan` to `AdvanceTime` | Yes | Remove the call. It now throws `ArgumentOutOfRangeException` |
| Tests that assert `TargetInvocationException` for a saga handler that throws synchronously | Yes | Assert the original exception type |
| Tests that depend on timeouts being lost after a canceled `AdvanceTime` call or a failing timeout handler | Yes | Expect the remaining timeouts in the next `AdvanceTime` call |
| Timeout handlers that keep requesting timeouts that are already due | Yes | Fix the handler or the test. The call now throws `InvalidOperationException` |
| Projects that reference an older version of `NServiceBus` or `Microsoft.Extensions.TimeProvider.Testing` | Yes | [Update the packages](#package-dependencies) to the minimum versions |
| Tests of handlers, behaviors, and message session operations | No | Nothing |

## Chained timeouts are handled in the same AdvanceTime call

Affected: scenario tests in which a timeout handler requests another timeout, and that assert the number of results returned by `AdvanceTime`. A saga that requests the next reminder from each reminder is a typical example.

Not affected: tests that only check whether a timeout fires and tests with sagas that do not chain timeouts. Calling `AdvanceTime` only once does not avoid the change, because the chained timeouts are now handled in that call.

Before: `AdvanceTime` handled only the timeouts that were already scheduled when the call started and due by the end of the advanced time. A timeout requested by a timeout handler was never handled in the same call, even when it was due within the advanced time. It waited for the next call.

After: `AdvanceTime` keeps handling timeouts until none is due by the end of the advanced time. A timeout requested by a timeout handler is scheduled from the due time of the timeout that requested it and handled in the same call when it is due.

For a saga that requests a new 10-minute reminder from each reminder, the three `AdvanceTime` calls in the following test return 1, 1, and 1 results in version 10, and 6, 0, and 0 results in version 11:

snippet: 10to11-chained-timeouts

To update a test, change the expected counts. To regain control between reminders, advance time in steps that are no longer than the reminder interval, or advance once and inspect the `SagaDataSnapshot` of each result:

snippet: 10to11-chained-timeouts-control

## Overdue timeouts are handled at the current time

Affected: tests that request a timeout for a time that has already passed, for example a saga that requests a timeout at an absolute `DateTimeOffset` that is earlier than `CurrentTime`. This also applies to delayed messages that the saga sends to itself with an absolute delivery time in the past.

Not affected: tests with timeouts that are in the future when they are requested.

Before: while an overdue timeout was handled, `CurrentTime` moved back to the due time of the timeout. A timeout that the handler requested was scheduled from that earlier time.

After: the clock cannot move backwards, so an overdue timeout is handled at `CurrentTime`. A timeout that the handler requests is scheduled relative to that time, so the next reminder is scheduled from when the overdue one was handled.

In the following test, the timeout is due at 8:30 AM but is requested at 9:00 AM. In version 10 the follow-up timeout was due at 8:45 AM and the next call handled it. In version 11 it is due at 9:15 AM:

snippet: 10to11-overdue-timeout

To update a test, calculate the follow-up due time from the time the overdue timeout was handled.

## Negative time spans are rejected

Affected: tests that pass a negative `TimeSpan` to `AdvanceTime`.

Before: the call moved `CurrentTime` backwards without an error.

After: the call throws an `ArgumentOutOfRangeException`.

snippet: 10to11-negative-advance-time

To update a test, remove the call. To test a saga at an earlier point in time, create a `TestableSaga` with an earlier `initialCurrentTime`.

## Synchronous handler exceptions are not wrapped

Affected: tests that assert `TargetInvocationException`, or its `InnerException`, for a saga handler that throws before it returns a task. For example, a handler method that is not `async` and throws directly.

Not affected: tests for handlers marked `async`. Those already threw the original exception.

Before: an exception thrown synchronously by a saga handler or timeout handler reached the test wrapped in a `TargetInvocationException`.

After: the original exception is thrown, as it is for `async` handlers. The original stack trace is preserved.

snippet: 10to11-synchronous-handler-exception

To update a test, assert the original exception type and remove the check of `InnerException`.

## Unhandled timeouts are kept

Affected: tests that cancel `AdvanceTime` or let a timeout handler throw, and that then continue to call `AdvanceTime` on the same `TestableSaga`.

Before: `AdvanceTime` removed all due timeouts from storage before it handled the first one. When the call was canceled or a timeout handler threw, the timeouts that had not been handled were lost. Later calls never handled them.

After: each timeout is removed right before it is handled. The remaining timeouts stay stored, and the next `AdvanceTime` call handles them when they are due. A timeout whose handler threw is not retried. After a cancellation or a failure, `CurrentTime` stays at the time the last timeout that started was handled. For an overdue timeout, that is the current time and not its due time.

To update a test that relied on lost timeouts, expect the remaining timeouts in the results of the next `AdvanceTime` call. To start with no stored timeouts, create a new `TestableSaga`.

## Runaway timeouts throw an exception

Affected: tests with timeout handlers that keep requesting timeouts that are already due, for example with a zero delay. Tests that schedule more than 1,000 timeouts for the same time are also affected, even when no handler requests a timeout.

Before: such a handler produced one result per `AdvanceTime` call.

After: `AdvanceTime` handles the new timeouts in the same call and throws an `InvalidOperationException` after 1,000 timeouts were handled without the clock moving forward. The count restarts when time moves forward, so schedules with many timeouts at different times are not affected.

To update a test, fix the handler so that it stops requesting due timeouts, or use a delay greater than zero.

## Package dependencies

NServiceBus.Testing version 11 requires NServiceBus version 10.2.9 or later. Version 10 required 10.2.0 or later.

`TestableSaga` is backed by a `FakeTimeProvider`. NServiceBus.Testing version 11 depends on the [`Microsoft.Extensions.TimeProvider.Testing`](https://www.nuget.org/packages/Microsoft.Extensions.TimeProvider.Testing) package, version 10.11.0 or later. The package is restored as a transitive dependency, so no package reference is needed to use `TestableSaga`. Projects that reference an older version of the package, for example through central package management, must update it. Tests that create their own `FakeTimeProvider` use the same package.

## New TestableSaga members

The following additions are source compatible with existing tests, with one exception that follows the list:

- A `TestableSaga` constructor that takes a `Func<TimeProvider, TSaga>` factory and an optional `FakeTimeProvider`.
- A `TestableSaga.TimeProvider` property.

A call that passes a `null` literal as the only constructor argument no longer compiles, because the call is ambiguous (CS0121). Omit the argument or cast it to `Func<TSaga>`.

The existing constructor and `CurrentTime` keep working. `CurrentTime` now reads from the time provider. See [saga scenario testing](/nservicebus/testing/saga-scenario-testing.md) for details.
