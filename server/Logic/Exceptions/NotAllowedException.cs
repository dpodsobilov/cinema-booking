namespace Logic.Exceptions;

/// <summary>
/// Выбрасываем при недоступности (невозможности) выполнения запроса
/// (например при проваленных проверках в обработчике)
/// </summary>
public class NotAllowedException : Exception
{
    public int Code { get; }
    public override string Message { get; }

    public NotAllowedException(string message)
    {
        Code = 405;
        Message = message;
    }
}