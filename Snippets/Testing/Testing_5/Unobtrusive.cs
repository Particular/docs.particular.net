using System;
using NServiceBus.Testing;

class Unobtrusive
{
    void Setup()
    {
        #region SetupConventionsForUnitTests

        Test.Initialize(
            customisations: config =>
            {
                var conventions = config.Conventions();
                conventions.DefiningMessagesAs(MyMessageTypeConvention);
            });

        #endregion
    }

    bool MyMessageTypeConvention(Type arg)
    {
        return true;
    }
}