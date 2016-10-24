using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.Plugin.CustomChecks;

class CustomChecks
{
    #region CustomCheck6
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

    #region PeriodicCheck6
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


    public void Foo()
    {
        #region CustomCheck_Configure_ServiceControl
        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.CustomCheckPlugin("ServiceControl_Queue");
        #endregion
    }

    public void Foo2()
    {
        #region CustomCheck_disable
        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.DisableFeature<ServiceControl.Features.CustomChecks>();
        #endregion
    }
}