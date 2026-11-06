namespace Minty.Logger;

public static partial class Logger
{
    private static void parseFormat(LoggerId id, string? message = null)
    {
        ArgumentNullException.ThrowIfNull(message);
        var now = DateTime.Now;
        string format = getFormat(id) ?? defaultFormat;
        string log = "";

        string buffer = "";
        bool isKeyword = false;
        foreach(var c in format)
        {
            if(isKeyword)
            {
                if(c == '{' && buffer == "{")
                {
                    log += '{';
                    isKeyword = false;
                    buffer = "";
                }
                else if(c != '}')
                {
                    buffer += c;
                }
                else
                {
                    buffer += '}';
                    switch(buffer.Trim())
                    {
                        // Mecros
                        case Keyword.Date:
                            log += $"{now.Date:yyyy-MM-dd}";
                            break;
                        case Keyword.Time:
                            log += $"{now.Hour}:{now.Minute}:{now.Second}.{now.Millisecond}";
                            break;
                        case Keyword.Level:
                            log += $"{id}";
                            break;

                        // Message
                        case Keyword.Message:
                            log += message;
                            break;

                        // Colors
                        case Keyword.Color.Red:
                            Console.Write(log);
                            log = "";
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case Keyword.Color.Blue:
                            Console.Write(log);
                            log = "";
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case Keyword.Color.Gray:
                            Console.Write(log);
                            log = "";
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        case Keyword.Color.Reset:
                            Console.Write(log);
                            log = "";
                            Console.ResetColor();
                            break;
                        default:
                            throw new ArgumentException($"Unknown Token: {buffer}");
                    }
                    isKeyword = false;
                    buffer = "";
                }
                continue;
            } else
            {
                if(c != '{') log += c;
                else { buffer += '{'; isKeyword = true; }
                continue;
            }
        }
        Console.WriteLine(log);
    }

    private static string? getFormat(LoggerId id)
    => id switch
    {
        LoggerId.Debug => Formats.debugFormat,
        LoggerId.Info => Formats.informationFormat,
        _ => null
    };
}