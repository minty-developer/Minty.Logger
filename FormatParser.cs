namespace Minty.Logger;

public static partial class Logger
{
    private static string parseFormat(LoggerId id, string? message = null)
    {
        ArgumentNullException.ThrowIfNull(message);
        string format = getFormat(id) ?? defaultFormat;
        string log = "";

        string buffer = "";
        bool isKeyword = false;
        foreach(var c in format)
        {
            if(isKeyword)
            {
                if(c == '{' && buffer.IsWhiteSpace()) { log += '{'; isKeyword = false; }
                else if(c != '}') buffer += c;
                else keywordHandler(ref log, ref buffer, ref isKeyword, message);
                continue;
            } else
            {
                if(c != '{') log += c;
                else isKeyword = true;
                continue;
            }
        }
        return log;
    }

    private static string? getFormat(LoggerId id)
    => id switch
    {
        LoggerId.Debug => Formats.debugFormat,
        LoggerId.Info => Formats.informationFormat,
        _ => null
    };

    private static void keywordHandler(ref string log, ref string buffer, ref bool isKeyword, string message)
    {
        switch(buffer)
        {
            // Mecros
            case Keyword.Date:
                log += DateTime.Now.Date;
                break;

            // Message
            case Keyword.Message:
                log += message;
                break;

            // Colors
            case Keyword.Color.Red:
                buffer = "";
                Console.Write(log);
                log = "";
                Console.ForegroundColor = ConsoleColor.Red;
                isKeyword = false;
                break;
            case Keyword.Color.Blue:
                buffer = "";
                Console.Write(log);
                log = "";
                Console.ForegroundColor = ConsoleColor.Blue;
                isKeyword = false;
                break;
            case Keyword.Color.Gray:
                buffer = "";
                Console.Write(log);
                log = "";
                Console.ForegroundColor = ConsoleColor.Gray;
                isKeyword = false;
                break;
            case Keyword.Color.Reset:
                buffer = "";
                Console.Write(log);
                log = "";
                Console.ResetColor();
                isKeyword = false;
                break;
            default:
                throw new ArgumentException($"Unknown Token: {buffer}");
        }
        buffer = "";
    }
}