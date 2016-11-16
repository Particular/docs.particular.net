namespace Core4.UpgradeGuides._4to5
{
    using NServiceBus;

    class Upgrade
    {

        public void FileShareDataBus(string databusPath)
        {
            #region 4to5FileShareDataBus

            var configure = Configure.With();
            configure.FileShareDataBus(databusPath);

            #endregion
        }

    }

}