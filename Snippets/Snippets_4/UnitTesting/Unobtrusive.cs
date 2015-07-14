namespace Snippets4.UnitTesting
{
    using System;
    using NServiceBus;
    using NServiceBus.Testing;

    public class Unobtrusive
    {
        public class Setup
        {
            public Setup()
            {
                #region SetupConventionsForUnitTests
                MessageConventionExtensions.IsMessageTypeAction = MyMessageTypeConvention;

                Test.Initialize();
                #endregion
            }

            bool MyMessageTypeConvention(Type arg)
            {
                return true;
            }
        }
    }
}