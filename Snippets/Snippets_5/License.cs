using NServiceBus;

public class License
{
    public void Simple()
    {
        #region LicenseV5
     
        var configuration = new BusConfiguration();

        configuration.LicensePath("PathToLicense");
        //or
        configuration.License("YourCustomLicenseText");

        #endregion
    }

}