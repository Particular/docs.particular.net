Note that only `ServerShared` has the [NServiceBus.Persistence.Sql NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.Sql) directly referenced. This will cause the script directory `ServerShared\bin\Debug\[TFM]\NServiceBus.Persistence.Sql\[Variant]` to be populated at build time.

This is a test for only for V6.

#variant 6-control
This is the control for V6
#end-variant
#variant 6-alternate
This is the alternate for V6
#end-variant
