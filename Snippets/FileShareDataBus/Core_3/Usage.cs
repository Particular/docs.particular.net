namespace Core3
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure, string databusPath)
        {
            #region FileShareDataBus

            configure.FileShareDataBus(databusPath);

            #endregion
        }

    }

}