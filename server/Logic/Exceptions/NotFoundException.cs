namespace Logic.Exceptions;

/// <summary>
/// Выбрасываем при отсуствии значения
/// (например при вытягивании null из БД)
/// </summary>
public class NotFoundException : Exception
{
    public int Code { get; }
    public override string Message { get; }

    public NotFoundException(string message)
    {
        Code = 404;
        Message = message;
    }
}