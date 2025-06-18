namespace Blackjack.Data.Other.Exceptions;

public class RenameProblemException : Exception, ICustomException
{
    public RenameProblemException(string message) : base(message){}
}