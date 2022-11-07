## Overriding the value of TransactionManager.MaxTimeout

### .NET Framework

The setting can be modified using a machine level-configuration file:

```xml
<system.transactions>
  <machineSettings maxTimeout="01:00:00" />
</system.transactions>
```

or using reflection:

snippet: override-transaction-manager-timeout-net-framework

### .NET Core

The setting can be modified using reflection:

snippet: override-transaction-manager-timeout-net-core