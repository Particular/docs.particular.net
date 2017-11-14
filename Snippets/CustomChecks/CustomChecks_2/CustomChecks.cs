using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.CustomChecks;

class CustomChecks
{
    #region CustomCheck
    public class MyCustomCheck :
        CustomCheck
    {
        public MyCustomCheck()
            : base("SomeId-StartUp", "SomeCategory")
        {

        }

        public override Task<CheckResult> PerformCheck()
        {
            if (SomeService.GetStatus())
            {
                return CheckResult.Pass;
            }

            return CheckResult.Failed("Some service is not available.");
        }
    }
    #endregion

    #region PeriodicCheck
    public class MyPeriodicCheck :
        CustomCheck
    {
        public MyPeriodicCheck()
            : base("SomeId-Periodic", "SomeCategory", TimeSpan.FromSeconds(5))
        {
        }

        public override Task<CheckResult> PerformCheck()
        {
            if (SomeService.GetStatus())
            {
                return CheckResult.Pass;
            }

            return CheckResult.Failed("Some service is not available.");
        }
    }
    #endregion

    public class SomeService
    {
        public static bool GetStatus()
        {
            return false;
        }
    }


    public void CustomCheck_Configure_ServiceControl()
    {
        #region CustomCheckNew_Enable
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
        endpointConfiguration.ReportCustomChecksTo(
            serviceControlQueue: "ServiceControl_Queue",
            timeToLive: TimeSpan.FromSeconds(10));
        #endregion
    }
}