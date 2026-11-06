namespace Minty.Log;

public static class Keyword
{
    public const string Date = "{date}";
    public const string Time = "{time}";
    public const string Message = "{message}";
    public const string Level = "{level}";
    public static class Color
    {
        public const string Red = "{Red}";
        public const string Yellow = "{Yellow}";
        public const string Green = "{Green}";
        public const string Blue = "{Blue}";
        public const string Gray = "{Gray}";
        public const string Reset = "{Reset}";

        public static string GetStatusCodeColor(int statusCode)
        {
            string buffer = Green;
            if(statusCode >= 300) buffer = Yellow;
            if(statusCode >= 400) buffer = Red;
            return buffer;
        }
    }
}

public enum LogLevel
{
    Debug,
    Info,
    Warn,
    Error
}

public enum LoggerId
{
    Default,
    Debug,
    Info,
    Success,
    Warn,
    Error,
    HttpReq,
    HttpRes,
    HttpStart,
    HttpEnd
}