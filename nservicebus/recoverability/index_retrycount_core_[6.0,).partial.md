
The total number of possible retries can be calculated with the following formula

```no-highlight
Total Attempts = (ImmediateRetries:NumberOfRetries + 1) * (DelayedRetries:NumberOfRetries + 1)
```

So for example given a variety of Immediate and Delayed here are the resultant possible attempts.

| ImmediateRetries | DelayedRetries | Total possible attempts |
|------------------|----------------|-------------------------|
| 1                | 1              | 4                       |
| 1                | 2              | 6                       |
| 1                | 3              | 8                       |
| 2                | 1              | 6                       |
| 3                | 1              | 8                       |
| 2                | 2              | 9                       |