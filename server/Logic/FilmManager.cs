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
}