#pragma warning disable 618
namespace Core6
{
    using System;
    using NServiceBus;

    class PerformanceMonitoring
    {

        void EnablingCriticalTime(EndpointConfiguration endpointConfiguration)
        {
            #region enable-criticaltime

            endpointConfiguration.EnableCriticalTimePerformanceCounter();

            #endregion
        }
        void EnablingSla(EndpointConfiguration endpointConfiguration)
        {
            #region enable-sla

            endpointConfiguration.EnableSLAPerformanceCounter(TimeSpan.FromMinutes(3));

            #endregion
        }


    }
}