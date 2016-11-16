```mermaid
graph LR
    A[ExchangeA] --> B[ExchangeB]
    A --> D[ExchangeD]
	B --> C[ExchangeC]
	B --> Q1[Queue1]
	D --> Q2[Queue2]
```

```
                 +-------------+
                 |            E|
                 | ExchangeA   |
                 |             |
                 +--+-------+--+
                    |       |
                    |       |
                    |       |
                    v       v

        +-------------+   +-------------+
        |            E|   |            E|
        | ExchangeB   |   | ExchangeD   |
        |             |   |             |
        +--+-------+--+   +----------+--+
           |       |                 |
           |       |                 |
           |       |                 |
           v       v                 v

+-------------+   +-------------+   +-------------+
|            E|   |            Q|   |            Q|
| ExchangeC   |   | Queue1      |   | Queue2      |
|             |   |             |   |             |
+-------------+   +-------------+   +-------------+

```