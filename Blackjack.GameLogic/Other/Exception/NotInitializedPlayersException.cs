namespace Blackjack.GameLogic.Other.Exception;
using System;

public class NotInitializedPlayersException : Exception
{
    public NotInitializedPlayersException()
        : base("Players are not initialized.")
    {
    }

    public NotInitializedPlayersException(string message)
        : base(message)
    {
    }
}