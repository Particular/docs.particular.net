namespace Contracts
{
    using System;

    public static class ZipCodeMappings
    {
        public static string Map(IHaveZipCode zipCode)
        {
            var zipCodeAsNumber = Convert.ToInt32(zipCode.ZipCode);
            // 00000..33000 => 33000 34000..66000 => 66000 67000..99000 => 99000
            if (zipCodeAsNumber >= 0 && zipCodeAsNumber <= 33000)
            {
                return "33000";
            }

            if (zipCodeAsNumber >= 34000 && zipCodeAsNumber <= 66000)
            {
                return "66000";
            }

            if (zipCodeAsNumber >= 67000 && zipCodeAsNumber <= 99000)
            {
                return "99000";
            }

            return null;
        }
    }
}