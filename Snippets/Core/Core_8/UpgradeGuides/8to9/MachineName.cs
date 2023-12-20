using NServiceBus.Support;

class MachineName
{
    void Override()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region core-8to9-machinename
        RuntimeEnvironment.MachineNameAction = () => "MyMachineName";
        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }
}