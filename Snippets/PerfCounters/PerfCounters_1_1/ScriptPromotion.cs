using NServiceBus.Metrics.PerformanceCounters;

#region perfcounter-script-all
[assembly: PerformanceCounterSettings(CSharp = true, Powershell = true)]
#endregion

/**
#region perfcounter-script-csharp
[assembly: PerformanceCounterSettings(CSharp = true, Powershell = false)]
#endregion

#region perfcounter-script-powershell
[assembly: PerformanceCounterSettings(CSharp = false, Powershell = true)]
#endregion

#region perfcounter-script-promotion
[assembly: PerformanceCounterSettings(ScriptPromotionPath = "$(SolutionDir)PromotedScripts")]
#endregion
**/
