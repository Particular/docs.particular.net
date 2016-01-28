namespace Snippets5
{
    using System;
    using System.Threading.Tasks;
    using ServiceControl.Plugin.CustomChecks;

    class CustomChecks
    {
        #region CustomCheck
        public class MyCustomCheck : CustomCheck
        {
            public MyCustomCheck()
                : base("SomeId-StartUp", "SomeCategory")
            {

            }

            public override Task PerformCheck()
            {
                if (SomeService.GetStatus())
                {
                    return ReportPass();
                }

                return ReportFailed("Some service is not available.");
            }
        }
        #endregion

        #region PeriodicCheck
        public class MyPeriodicCheck : PeriodicCheck
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

    }
}
