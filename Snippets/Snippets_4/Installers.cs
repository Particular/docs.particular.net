namespace Snippets4
{
    using NServiceBus;
    using NServiceBus.Installation.Environments;

    public class ForInstallationOn
    {
        public void Simple()
        {
            #region Installers

            Configure.With()
                .UnicastBus()
                .CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

            #endregion
        }



    }
}