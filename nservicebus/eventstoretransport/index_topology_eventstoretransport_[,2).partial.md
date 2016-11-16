```mermaid
graph LR
    A[Square Rect] -- Link text --> B((Circle))
    A --> C(Round Rect)
    B --> D{Rhombus}
    C --> D
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