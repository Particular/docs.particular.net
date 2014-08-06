using NServiceBus;


public class CustomLicense
{
    public void Simple()
    {
        // start code CustomLicenseV4
        Configure.With().LicensePath("PathToLicense");

        //or

        Configure.With().License("YourCustomLicenseText");
        // end code CustomLicenseV4
    }

}