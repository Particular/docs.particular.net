using System;
using System.Threading;
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

        public override Task<CheckResult> PerformCheck(CancellationToken cancellationToken = default)
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

        public override Task<CheckResult> PerformCheck(CancellationToken cancellationToken = default)
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

    #region CustomCheckFailed
    public class CustomCheckFailed
    {
        /// <summary>
        /// The id for the custom check provided by the user.
        /// </summary>
        public string CustomCheckId { get; set; }

        /// <summary>
        /// The custom check category provided by the user.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The reason provided by the user for the failure.
        /// </summary>
        public string FailureReason { get; set; }

        /// <summary>
        /// The date and time the check failed.
        /// </summary>
        public DateTime FailedAt { get; set; }

        /// <summary>
        /// The name of the endpoint
        /// </summary>
        public string EndpointName { get; set; }

        /// <summary>
        /// The unique identifier for the host that runs the endpoint
        /// </summary>
        public Guid HostId { get; set; }

        /// <summary>
        /// The name of the host
        /// </summary>
        public string Host { get; set; }
    }
    #endregion

    #region CustomCheckSucceeded
    public class CustomCheckSucceeded
    {
        /// <summary>
        /// The id for the custom check provided by the user.
        /// </summary>
        public string CustomCheckId { get; set; }

        /// <summary>
        /// The custom check category provided by the user.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The date and time the check passed.
        /// </summary>
        public DateTime SucceededAt { get; set; }

        /// <summary>
        /// The name of the endpoint
        /// </summary>
        public string EndpointName { get; set; }

        /// <summary>
        /// The unique identifier for the host that runs the endpoint
        /// </summary>
        public Guid HostId { get; set; }

        /// <summary>
        /// The name of the host
        /// </summary>
        public string Host { get; set; }
    }
    #endregion
}