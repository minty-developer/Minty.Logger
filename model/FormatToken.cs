namespace Minty.Log;

public static partial class Logger
{
    private class Token(TokenId id, object? value = null)
    {
        public TokenId Id = id;
        public object? Value = value;
    }
}