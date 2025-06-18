namespace Blackjack.Data.Other.Exceptions;

public class NotFoundInDatabaseException : Exception, ICustomException
{
    public NotFoundInDatabaseException(string message) : base(message){}
}