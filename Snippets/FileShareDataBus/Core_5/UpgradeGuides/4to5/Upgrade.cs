namespace Core5.UpgradeGuides._4to5
{
    using NServiceBus;

    public class Upgrade
    {

        void FileShareDataBus(BusConfiguration busConfiguration, string databusPath)
        {
            #region 4to5FileShareDataBus

            var dataBus = busConfiguration.UseDataBus<FileShareDataBus>();
            dataBus.BasePath(databusPath);

            #endregion
        }

    }
}