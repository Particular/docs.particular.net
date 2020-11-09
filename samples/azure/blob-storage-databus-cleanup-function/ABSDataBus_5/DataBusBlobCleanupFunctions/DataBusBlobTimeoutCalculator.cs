using System;
using System.Globalization;
using Microsoft.Azure.Storage.Blob;

public class DataBusBlobTimeoutCalculator
{
    public TimeSpan DefaultTtl { get; private set; }

    public DataBusBlobTimeoutCalculator()
    {
        var defaultTtlSettingValue = Environment.GetEnvironmentVariable("DefaultTimeToLiveInSeconds");
        if (defaultTtlSettingValue == null)
        {
            throw new Exception("Could not read the DefaultTimeToLiveInSeconds settings value.");
        }

        if (!long.TryParse(defaultTtlSettingValue, out var defaultTtlSeconds))
        {
            throw new Exception($"Could not parse the DefaultTimeToLiveInSeconds value '{defaultTtlSettingValue}");
        }

        DefaultTtl = TimeSpan.FromSeconds(defaultTtlSeconds);
    }

    #region GetValidUntil
    public DateTime GetValidUntil(CloudBlockBlob blockBlob)
    {
        if (blockBlob.Metadata.TryGetValue("ValidUntilUtc", out var validUntilUtcString))
        {
            return ToUtcDateTime(validUntilUtcString);
        }

        return DateTime.MaxValue;
    }

    #endregion

    /// <summary>
    /// Converts the <see cref="DateTime" /> to a <see cref="string" /> suitable for transport over the wire.
    /// </summary>
    public string ToWireFormattedString(DateTime dateTime)
    {
        return dateTime.ToUniversalTime().ToString(format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts a wire-formatted <see cref="string" /> from <see cref="ToWireFormattedString" /> to a UTC
    /// <see cref="DateTime" />.
    /// </summary>
    public DateTime ToUtcDateTime(string wireFormattedString)
    {
        if (string.IsNullOrWhiteSpace(wireFormattedString))
        {
            throw new ArgumentNullException(nameof(wireFormattedString));
        }

        if (wireFormattedString.Length != format.Length)
        {
            throw new FormatException(errorMessage);
        }

        var year = 0;
        var month = 0;
        var day = 0;
        var hour = 0;
        var minute = 0;
        var second = 0;
        var microSecond = 0;

        for (var i = 0; i < format.Length; i++)
        {
            var digit = wireFormattedString[i];

            switch (format[i])
            {
                case 'y':
                    Guard(digit);
                    year = year * 10 + (digit - '0');
                    break;

                case 'M':
                    Guard(digit);
                    month = month * 10 + (digit - '0');
                    break;

                case 'd':
                    Guard(digit);
                    day = day * 10 + (digit - '0');
                    break;

                case 'H':
                    Guard(digit);
                    hour = hour * 10 + (digit - '0');
                    break;

                case 'm':
                    Guard(digit);
                    minute = minute * 10 + (digit - '0');
                    break;

                case 's':
                    Guard(digit);
                    second = second * 10 + (digit - '0');
                    break;

                case 'f':
                    Guard(digit);
                    microSecond = microSecond * 10 + (digit - '0');
                    break;
            }
        }

        return AddMicroseconds(new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc), microSecond);

        void Guard(char digit)
        {
            if (digit >= '0' && digit <= '9')
            {
                return;
            }

            throw new FormatException(errorMessage);
        }

        DateTime AddMicroseconds(DateTime self, int microseconds)
        {
            return self.AddTicks(microseconds * ticksPerMicrosecond);
        }
    }

    const string format = "yyyy-MM-dd HH:mm:ss:ffffff Z";
    const string errorMessage = "String was not recognized as a valid DateTime.";
    const int ticksPerMicrosecond = 10;
}
