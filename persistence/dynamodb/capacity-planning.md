---
title: AWS DynamoDB persistence capacity planning
summary: Provides AWS DynamoDB capacity planning guidance
component: DynamoDB
reviewed: 2026-04-27
---

AWS DynamoDB charges for data access in terms of [read capacity units (RCUs) and write capacity units (WCUs)](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/provisioned-capacity-mode.html). Understanding how NServiceBus maps [saga](/nservicebus/sagas/) and [outbox](/nservicebus/outbox/) operations to DynamoDB API calls helps estimate costs and plan capacity for both provisioned throughput and on-demand pricing modes.

## Capacity unit fundamentals

| Operation type | Cost |
|---|---|
| Strongly consistent read | 1 RCU per 4 KB (rounded up) |
| Eventually consistent read | 0.5 RCU per 4 KB (rounded up) |
| Transactional read | 2 RCUs per 4 KB (rounded up) |
| Standard write | 1 WCU per 1 KB (rounded up) |
| Transactional write | 2 WCUs per 1 KB (rounded up) |

All write operations performed as part of the `TransactWriteItems` API call are charged at the transactional rate (2× the standard write cost). Standard `UpdateItemAsync`, `DeleteItemAsync`, and `BatchWriteItemAsync` calls are charged at the standard rate.

In on-demand mode, the same ratios apply but are expressed as read request units (RRUs) and write request units (WRUs) billed per actual request rather than provisioned hourly.

## Sagas

### Optimistic concurrency (default)

[Optimistic concurrency](/persistence/dynamodb/sagas.md#saga-concurrency) is the default mode. Each message handler that loads a saga performs the following operations:

#### Reading a saga

A `GetItem` call is issued. By default, this is a strongly consistent read. Eventual consistency can be enabled via `UseEventuallyConsistentReads`.

- Strongly consistent: `⌈saga item size / 4 KB⌉` RCUs
- Eventually consistent: `⌈saga item size / 4 KB⌉ × 0.5` RCUs

#### Saving, updating, or completing a saga

The saga write is added to the `TransactWriteItems` batch and committed alongside outbox operations (if the outbox is enabled) at the end of message handler execution. This is one transactional write per saga:

- `⌈saga item size / 1 KB⌉ × 2` WCUs

If a concurrent modification is detected at commit time (the version check condition fails), the transaction is rejected and NServiceBus retries the message.

#### Summary per message (saga only, no outbox)

| Step | API call | Capacity |
|---|---|---|
| Load saga | `GetItem` | `⌈saga size / 4 KB⌉` RCUs |
| Save / update _(alternative)_ | `TransactWriteItems` | `⌈saga size / 1 KB⌉ × 2` WCUs |
| Complete saga _(alternative)_ | `TransactWriteItems` | `⌈saga size / 1 KB⌉ × 2` WCUs |

> [!NOTE]
> Save / update and Complete saga are mutually exclusive — only one applies per message handler invocation.

### Pessimistic locking (optional)

When [pessimistic locking](/persistence/dynamodb/sagas.md#saga-concurrency) is enabled, the saga is locked before the message handler runs to prevent concurrent processing.

#### Acquiring the lock

A conditional `UpdateItem` call attempts to set a lease timestamp on the saga record. This is a standard (non-transactional) write that also returns the full updated item, so no separate read is needed:

- `⌈saga item size / 1 KB⌉` WCUs (standard write)

If the lock is already held by another handler, the call is retried with random jitter (100–300 ms) until the `LeaseAcquisitionTimeout` is reached. Each failed attempt still consumes WCUs.

#### Releasing the lock

Lock release is handled differently depending on whether the session was committed:

- _Session committed_ (saga was saved, updated, or completed): no extra call. The commit writes the saga data with the lease already cleared.
- _Session not committed_ (handler threw an exception or read-only path):
  - Existing saga: `UpdateItem` to clear the lease — `⌈saga item size / 1 KB⌉` WCUs (standard write)
  - New saga (lock-only record): `DeleteItem` to remove the lock record — 1 WCU

#### Summary per message (pessimistic locking, no outbox)

| Step | API call | Capacity |
|---|---|---|
| Acquire lock and read saga | `UpdateItem` | `⌈saga size / 1 KB⌉` WCUs |
| Save / update _(alternative)_ | `TransactWriteItems` | `⌈saga size / 1 KB⌉ × 2` WCUs |
| Complete saga _(alternative)_ | `TransactWriteItems` | `⌈saga size / 1 KB⌉ × 2` WCUs |
| Release lock (if no commit) | `UpdateItem` or `DeleteItem` | `⌈saga size / 1 KB⌉` WCUs |

> [!NOTE]
> The lock acquisition uses `ReturnValues = ALL_NEW`, so the updated item (including saga data) is returned inline. No separate `GetItem` is needed. Save / update and Complete saga are mutually exclusive — only one applies per message handler invocation.

## Outbox

The outbox stores incoming message IDs and the resulting outgoing transport operations so that handler execution can be made idempotent. Each incoming message passes through four outbox lifecycle steps.

### Step 1 — Deduplication check

Before running the message handler, NServiceBus issues a `Query` with `ConsistentRead = true` to check whether the incoming message has already been processed.

The query returns all items within the outbox partition for that message ID: one metadata item, plus one item per outgoing transport operation (stored separately to stay within the 400 KB per-item limit).

- New message (not yet seen): 1 item returned — `⌈metadata item size / 4 KB⌉` RCUs
- Duplicate message (already dispatched): 1 item returned (metadata only, operations already deleted) — `⌈metadata item size / 4 KB⌉` RCUs
- Duplicate message (not yet dispatched): `1 + N` items — `⌈(metadata + N operations) / 4 KB⌉` RCUs

When a duplicate is detected, processing stops here with no further capacity consumed.

### Step 2 — Storing outbox records (as part of transaction)

When the message handler completes successfully, the outbox records are written atomically together with any saga changes via a single `TransactWriteItems` call. The outbox contributes `1 + N` items to the transaction:

- 1 metadata item (small — contains message ID, operation count, dispatched flag)
- 1 item per outgoing transport operation (contains message headers, dispatch options, and body)

Transactional write cost per item: `⌈item size / 1 KB⌉ × 2` WCUs

> [!NOTE]
> DynamoDB limits `TransactWriteItems` to 100 items total. Each saga counts as one item. The outbox counts as `1 + N` items where N is the number of outgoing messages.

### Step 3 — Marking as dispatched

After outgoing messages are dispatched to the transport, NServiceBus marks the outbox record as dispatched and schedules it for TTL cleanup:

1. `UpdateItem` on the metadata record to set `Dispatched = true`, `DispatchedAt`, and the TTL attribute — 1 WCU (standard write, small item)
2. `BatchWriteItem` to delete the N transport operation items — `N` WCUs minimum (standard writes, each key-only deletion is 1 WCU)

`BatchWriteItem` is used intentionally here (rather than a transaction) because the dispatch operation is idempotent and does not require atomicity, avoiding the 2× transactional write cost.

### Step 4 — TTL expiry

After the deduplication retention period (7 days by default), DynamoDB automatically expires the metadata record. TTL deletions are free and do not consume WCUs.

### Summary per incoming message (outbox only, N outgoing messages)

| Step | API call | Capacity |
|---|---|---|
| Deduplication check | `Query` (consistent) | `⌈metadata / 4 KB⌉` RCUs |
| Store records | `TransactWriteItems` | `⌈metadata / 1 KB⌉ × 2 + Σ ⌈op[i] / 1 KB⌉ × 2` WCUs |
| Mark dispatched | `UpdateItem` | 1 WCU |
| Delete operations | `BatchWriteItem` | N WCUs (minimum) |
| TTL expiry | Automatic | Free |

## Saga and outbox combined

When sagas and the outbox are both enabled, their writes are committed in a single `TransactWriteItems` call. This means saga and outbox writes are charged together in one transaction.

For a handler that loads a saga, sends N messages, and updates the saga:

| Step | API calls | Capacity |
|---|---|---|
| Deduplication check | `Query` | `⌈metadata / 4 KB⌉` RCUs |
| Load saga | `GetItem` | `⌈saga size / 4 KB⌉` RCUs |
| Commit (saga update + outbox) | `TransactWriteItems` | `(⌈saga / 1 KB⌉ + ⌈metadata / 1 KB⌉ + Σ ⌈op[i] / 1 KB⌉) × 2` WCUs |
| Mark dispatched | `UpdateItem` | 1 WCU |
| Delete outbox operations | `BatchWriteItem` | N WCUs (minimum) |

When pessimistic locking is enabled, add 1 standard `UpdateItem` for the lock acquisition.

## Item size estimation

Actual costs depend on the size of stored data. The following baselines can be used as a starting point:

| Item | Approximate size |
|---|---|
| Outbox metadata record | ~200–400 bytes (no body) |
| Outbox transport operation | headers + dispatch options + message body |
| Saga record | saga data properties + ~100 bytes of NServiceBus metadata |

Because DynamoDB rounds reads up to the next 4 KB boundary and writes up to the next 1 KB boundary, small items are charged at the minimum unit. A saga data record under 1 KB still costs 1 WCU (2 WCUs transactionally), and an outbox metadata record under 4 KB still costs 1 RCU (2 RCUs transactionally).

## Capacity planning approach

1. Measure saga and outbox item sizes in a test environment (e.g., via the DynamoDB console or CloudWatch metrics).
2. Apply the formulas above to calculate RCUs and WCUs per incoming message.
3. Multiply by expected message throughput (messages per second) to get the required provisioned capacity, or use those figures to estimate on-demand costs.
4. Account for retries due to optimistic concurrency conflicts or transient throttling — each retry adds one full set of operations.

For provisioned mode, enable [DynamoDB auto scaling](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/AutoScaling.html) to handle traffic spikes without over-provisioning. For unpredictable or spiky workloads, on-demand mode avoids capacity planning overhead at the cost of a higher per-request price.
