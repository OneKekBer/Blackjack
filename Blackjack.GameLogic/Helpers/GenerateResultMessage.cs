namespace Blackjack.GameLogic.Helpers;

public static class MessagesGenerator
{
    public static string GenerateResultMessage(List<string> winnersName) // remove in other file
    {
        return winnersName.Count switch
        {
            0 => "Nobody won the game.",
            1 => $"{winnersName[0]} won the game.",
            _ => $"{string.Join(", ", winnersName)} won the game."
        };
    }
}