namespace Logic;

public static class FilmManager
{
    public static string GetDuration(string hours, string minutes)
    {
        var minutesValue = int.Parse(minutes);
        var hoursValue = int.Parse(hours);

        var minutesWord = "минут";
        if (minutesValue % 10 == 1 && minutesValue != 11)
        {
            minutesWord = "минута";
        }
        else if ((minutesValue % 10 == 2 || minutesValue % 10 == 3 || minutesValue % 10 == 4) &&
                 (minutesValue < 10 || minutesValue > 20))
        {
            minutesWord = "минуты";
        }

        var hoursWord = "часов";
        if ((hoursValue % 10 == 1 && hoursValue != 11) && (hoursValue < 10 || hoursValue > 20))
        {
            hoursWord = "час";
        }
        else if ((hoursValue % 10 == 2 || hoursValue % 10 == 3 || hoursValue % 10 == 4) &&
                 (hoursValue < 10 || hoursValue > 20))
        {
            hoursWord = "часа";
        }

        return $"{hoursValue} {hoursWord} {minutesValue} {minutesWord}";
    }
    
    public static byte[] StringToByteArray(String str)
    {
        var numberChars = str.Length;
        var bytes = new byte[numberChars / 2];
        for (var i = 0; i < numberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
        return bytes;
    }
}