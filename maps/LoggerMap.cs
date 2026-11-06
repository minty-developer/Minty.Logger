namespace Minty.Log;

public static partial class Logger
{
    private static Dictionary<string, ConsoleColor> colorMap = new()
    {
        {Keyword.Color.Red, ConsoleColor.Red},
        {Keyword.Color.Yellow, ConsoleColor.Yellow},
        {Keyword.Color.Green, ConsoleColor.Green},
        {Keyword.Color.Blue, ConsoleColor.Blue},
        {Keyword.Color.Gray, ConsoleColor.Gray},
    };

    private static Dictionary<LoggerId, Token[]?> formatMap = new()
    {
        {LoggerId.Debug, [new(TokenId.Text, "                              "), new(TokenId.Level), new(TokenId.Text), new(TokenId.Text, " "), new(TokenId.Message)]},
        {LoggerId.Success, null},
        {LoggerId.Info, null},
        {LoggerId.Warn, null},
        {LoggerId.Error, null},
        {LoggerId.HttpReq, null},
        {LoggerId.HttpRes, null},
        {LoggerId.HttpStart, null},
        {LoggerId.HttpEnd, null}
    };

    private static Dictionary<LoggerId, ConsoleColor> levelColorMap = new()
    {
        {LoggerId.Debug, ConsoleColor.White},
        {LoggerId.Info, ConsoleColor.Blue},
        {LoggerId.Success, ConsoleColor.Green},
        {LoggerId.Warn, ConsoleColor.Yellow},
        {LoggerId.Error, ConsoleColor.Red}
    };
}