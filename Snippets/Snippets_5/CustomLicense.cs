using NServiceBus;

public class CustomLicense
{
    public void Simple()
    {
        // start code CustomLicenseV5
        Configure.With(builder => builder.LicensePath("PathToLicense"));

        //or

        Configure.With(builder => builder.License("YourCustomLicenseText"));
        // end code CustomLicenseV5
    }

}