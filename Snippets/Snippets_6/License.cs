namespace Snippets6
{
    using NServiceBus;

    public class License
    {
        public void Simple()
        {
            #region License
     
            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.LicensePath("PathToLicense");
            //or
            busConfiguration.License("YourCustomLicenseText");

            #endregion
        }

    }
}