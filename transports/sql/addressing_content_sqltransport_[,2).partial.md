## Format

The SQL Server Transport address has following canonical form

```
table
```

The table name is an bracket-delimited identifier without the surrounding square brackets. Whitespace characters are allowed. Brackets must be escaped e.g. `my table` and `my]]table` are legal values. The surrounding brackets are added automatically by SQL Server transport when executing the SQL statements. `@` is not a valid character. All characters after `@` (including `@`) are omitted when parsing addresses e.g. `my t@ble` is parsed as `my t`.


## Resolution

The address is resolved into a qualified table name that includes both table name and its schema. The schema has to be configured.


### Schema

A schema for a destination endpoint can be specified using a connection string convention. In the example below the connection string `NServiceBus/Transport/Billing` overrides the default if sending to the `Billing` queue, in which case `billingSchema` is used.

snippet: sqlserver-non-standard-schema-messagemapping
