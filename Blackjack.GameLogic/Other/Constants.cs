namespace Blackjack.GameLogic.Other;

public static class Constants
{
    public const int Limit = 21;
    
    public static readonly Dictionary<string, int> RankValues = new ()
    {
        { "Ace", 11 },
        { "King", 10 },
        { "Queen", 10 },
        { "Jack", 10 },
        { "Ten", 10 },
        { "Nine", 9 },
        { "Eight", 8 },
        { "Seven", 7 },
        { "Six", 6 },
    };
}