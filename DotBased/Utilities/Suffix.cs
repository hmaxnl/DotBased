namespace DotBased.Utilities;

public static class Suffix
{
    private static readonly string[] SizeSuffixes =
        ["bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];
    
    public static string BytesToSizeSuffix(long bytes, int decimalPlaces = 1)
    {
        if (decimalPlaces < 0)
            decimalPlaces = 1;
        switch (bytes)
        {
            case < 0:
                return "-" + BytesToSizeSuffix(-bytes, decimalPlaces);
            case 0:
                return string.Format("{0:n" + decimalPlaces + "} bytes", 0);
        }

        int mag = (int)Math.Log(bytes, 1024);

        decimal adjustedSize = (decimal)bytes / (1L << (mag * 10));

        if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
        {
            mag += 1;
            adjustedSize /= 1024;
        }
        return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
    }
}