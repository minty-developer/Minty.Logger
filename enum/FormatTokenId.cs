namespace Minty.Log;

public static partial class Logger
{
    private enum TokenId
    {
        Text,
        Date,
        Time,
        Level,
        LevelColor,
        Color,
        ColorReset,
        Message,
    }
}