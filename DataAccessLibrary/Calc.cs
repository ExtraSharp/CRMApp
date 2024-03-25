namespace DataAccessLibrary;

public static class Calc
{
    public static long ConvertToTimestamp(DateTime value)
    {
        DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var elapsedTime = value - epoch.ToLocalTime();
        return (long)elapsedTime.TotalSeconds;
    }
    public static DateOnly TimeStampToDateOnly(double unixTimeStamp)
    {
        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();

        var output = DateOnly.FromDateTime(dateTime);

        return output;
    }
    public static DateTime TimeStampToDateTime(double unixTimeStamp)
    {
        DateTime output = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        output = output.AddSeconds(unixTimeStamp).ToLocalTime();

        return output;
    }

    public static TimeOnly TimeStampToTimeOnly(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();

        var output = TimeOnly.FromDateTime(dateTime);

        return output;
    }
}