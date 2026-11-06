namespace Minty.Logger;

public static partial class Logger
{
    private static string defaultFormat = "";

    private static class Formats
    {
        public static string? debugFormat = null;
        public static string? informationFormat = null;
    }

    public static class Keyword
    {
        public const string Date = "{date}";
        public const string Time = "{time}";
        public const string Message = "{message}";
        public const string Level = "{level}";
        public static class Color
        {
            public const string Red = "{Red}";
            public const string Blue = "{Blue}";
            public const string Gray = "{Gray}";
            public const string Reset = "{Reset}";
        }
    }
}