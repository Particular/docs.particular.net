namespace Snippets4
{
    using NServiceBus;

    public class CustomLicense
    {
        public void Simple()
        {
            #region License
            Configure.With().LicensePath("PathToLicense");
            //or
            Configure.With().License("YourCustomLicenseText");
            #endregion
        }

    }
}