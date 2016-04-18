namespace Core5
{
    using NServiceBus;

    class License
    {
        License(BusConfiguration busConfiguration)
        {
            #region License
     
            busConfiguration.LicensePath("PathToLicense");
            //or
            busConfiguration.License("YourCustomLicenseText");

            #endregion
        }

    }
}