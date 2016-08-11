The total number of possible retries can be calculated with the following formula

```no-highlight
Total Attempts = MAX(1, (ImmediateRetries:NumberOfRetries)) * (DelayedRetries:NumberOfRetries + 1)
```

So for example given a variety of Immediate and Delayed here are the resultant possible attempts.

| ImmediateRetries: NumberOfRetries | DelayedRetries: NumberOfRetries | Total possible attempts |
|----------------------------------|--------------------------------|-------------------------|
| 1                                | 1                              | 2                       |
| 1                                | 2                              | 3                       |
| 1                                | 3                              | 4                       |
| 2                                | 1                              | 4                       |
| 3                                | 1                              | 6                       |
| 2                                | 2                              | 6                       |