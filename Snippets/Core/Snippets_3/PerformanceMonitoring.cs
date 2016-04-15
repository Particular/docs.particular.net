namespace Core3
{
    using NServiceBus;

    class PerformanceMonitoring
    {
        void EnablingCriticalTime(Configure configure)
        {
            #region enable-criticaltime
            // in this version there was no granular control over individual counters
            configure.EnablePerformanceCounters();
            #endregion
        }
        void EnablingSla(Configure configure)
        {
            #region enable-sla
            // in this version there was no granular control over individual counters
            configure.EnablePerformanceCounters();
            #endregion
        }

    }
}