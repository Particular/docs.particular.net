using NServiceBus;

public class License
{
    public void Simple()
    {
        #region License
     
        var configuration = new BusConfiguration();

        configuration.LicensePath("PathToLicense");
        //or
        configuration.License("YourCustomLicenseText");

        #endregion
    }

}