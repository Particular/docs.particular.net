---
title: How to Centralize All Unobtrusive Declarations?
summary: Don&#39;t repeat yourself when writing unobtrusive declarations; use WantToRunBeforeConfiguration.
tags:
- Unobtrusive
- DRY
---

When working with NServiceBus in unobtrusive mode you may feel that you are repeating the conventions over and over again on all the endpoints.

The [IWantToRunBeforeConfiguration](https://github.com/NServiceBus/NServiceBus/blob/develop/src/NServiceBus.Core/IWantToRunBeforeConfiguration.cs) interface is a great help when embracing the DRY (don't repeat yourself) principle. 

Just define your implementation in an assembly referenced by all the endpoints:

```C#
public class UnobtrusiveConventions : IWantToRunBeforeConfiguration
{
    public void Init()
    {
        Configure.Instance
            .DefiningCommandsAs(t => t.Namespace != null
                && t.Namespace.StartsWith("MyCompany") 
                && t.Namespace.EndsWith("Commands"))
            .DefiningEventsAs(t => t.Namespace != null
                && t.Namespace.StartsWith("MyCompany") 
                && t.Namespace.EndsWith("Events"));
    }
}
```




