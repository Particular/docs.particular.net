---
title: "Bootstrapping Async Main"
suppressRelated: true
reviewed: 2017-09-18
---

[Enabling C# 7.1 features](https://www.meziantou.net/2017/08/24/3-ways-to-enable-c-7-1-features) on a project enables the use of an async `Main()` method in console applications:

```csharp
class Program
{
    static async Task Main()
    {
        // Start application here
    }
}
```

If C# 7.1 cannot be used, async behavior can be bootstrapped using `.GetAwaiter().GetResult()`:

```csharp
class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }


    static async Task AsyncMain()
    {
        // Start application here
    }
}
```

Using this method, any exceptions thrown are not needlessly wrapped in an `AggregateException`, making debugging easier.