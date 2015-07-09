namespace Snippets5.UnitTesting
{
    using System;
    using NServiceBus.Testing;

    public class Unobtrusive
    {
        public class Setup
        {
            public Setup()
            {
                #region SetupConventionsForUnitTests
                Test.Initialize(config => config.Conventions().DefiningMessagesAs(MyMessageTypeConvention));
                #endregion
            }

            bool MyMessageTypeConvention(Type arg)
            {
                return true;
            }
        }
    }
}