using System;
using NServiceBus;
using NServiceBus.Testing;

class Unobtrusive
{
    Unobtrusive()
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