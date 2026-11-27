---
title: SQL Persistence Upgrade Version 9 to 10
summary: Migration instructions on how to upgrade to SQL Persistence version 10
reviewed: 2026-11-27
component: SqlPersistence
related:
- persistence/sql
- persistence/sql/saga
isUpgradeGuide: true
---

Version 10 turns APIs that are obsolete in version 9 into errors. Endpoints that compile without obsolete warnings on the latest version 9 do not need any changes for the APIs listed in this guide.

| Change | Action required |
|--------|-----------------|
| [`StringBuilder` overload of `WriterCreator`](#stringbuilder-overload-of-writercreator) | Use the `TextWriter` overload |

## `StringBuilder` overload of `WriterCreator`

The `SagaSettings.WriterCreator` overload that accepts a `Func<StringBuilder, JsonWriter>` is obsolete with a warning starting in version 9.1 and generates an error in version 10. It will be removed in version 11.

Endpoints that do not call `WriterCreator` are not affected. No action is required.

Endpoints that [customize the saga JSON writer](/persistence/sql/saga.md#json-net-settings-custom-writer) with the `StringBuilder` overload must use the overload that accepts a `Func<TextWriter, JsonWriter>`, added in version 9.1, and pass the `TextWriter` to the `JsonWriter`. Affected code shows the obsolete warning `CS0618` when compiled against version 9.1 or later.

Before (version 9):

```csharp
var sagaSettings = persistence.SagaSettings();
sagaSettings.WriterCreator(
    writerCreator: builder =>
    {
        var writer = new StringWriter(builder);
        return new JsonTextWriter(writer)
        {
            IndentChar = '\t',
            Indentation = 1
        };
    });
```

After (version 10):

```csharp
var sagaSettings = persistence.SagaSettings();
sagaSettings.WriterCreator(
    writerCreator: textWriter =>
    {
        return new JsonTextWriter(textWriter)
        {
            IndentChar = '\t',
            Indentation = 1
        };
    });
```
