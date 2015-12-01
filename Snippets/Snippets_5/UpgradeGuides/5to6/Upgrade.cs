namespace Snippets5.UpgradeGuides._5to6
{
    using System;
    using NServiceBus;

    public class Upgrade
    {
        public void CriticalError()
        {
            // ReSharper disable RedundantDelegateCreation

            #region 5to6CriticalError

            var busConfiguration = new BusConfiguration();
            busConfiguration.DefineCriticalErrorAction(
                new Action<string, Exception>((error, exception) => {
                    // place you custom handling here 
                }));

            #endregion

            // ReSharper restore RedundantDelegateCreation
        }
    }
}