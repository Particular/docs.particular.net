namespace Core5.UpgradeGuides._4to5
{
    using NServiceBus;

    public class Upgrade
    {
        void EncryptionServiceSimple(BusConfiguration busConfiguration)
        {
            #region 4to5EncryptionServiceSimple

            busConfiguration.RijndaelEncryptionService();

            #endregion
        }

    }
}