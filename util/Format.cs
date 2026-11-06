using System.Text;

namespace Minty.Log;

public static partial class Logger
{
    private static Token[] defaultFormat = [
        new(TokenId.Color, ConsoleColor.Gray),
        new(TokenId.Text, "["),
        new(TokenId.Date),
        new(TokenId.Text, " "),
        new(TokenId.Time),
        new(TokenId.Text, "]"),
        new(TokenId.ColorReset),
        new(TokenId.LevelColor),
        new(TokenId.Text, " ["),
        new(TokenId.Level),
        new(TokenId.Text, "] "),
        new(TokenId.ColorReset),
        new(TokenId.Message)
    ];

    private static void logWithFormat(LoggerId id, string message)
    {
        Token[] format = getFormat(id) ?? defaultFormat;
        var log = new StringBuilder();
        var Now = DateTime.Now;
        foreach(var token in format)
        {
            switch(token.Id)
            {
                case TokenId.Text: { log.Append(token.Value); break; }
                case TokenId.Message: { log.Append(message); break; }
                case TokenId.Level: { log.Append(token.Value ?? id.ToString()); break;}
                case TokenId.Date: { log.Append(Now.ToString("yyyy-MM-dd")); break; }
                case TokenId.Time: { log.Append(Now.ToString("HH:mm:ss.fff")); break; }
                case TokenId.LevelColor: { Console.Write(log.ToString()); Console.ForegroundColor = levelColorMap.GetValueOrDefault(id); log.Clear(); break; }
                case TokenId.Color: { Console.Write(log.ToString()); Console.ForegroundColor = (ConsoleColor)token.Value!; log.Clear(); break; }
                case TokenId.ColorReset: { Console.Write(log.ToString()); Console.ResetColor(); log.Clear(); break; }
            }
        }
        Console.WriteLine(log.ToString());
    }

    private static Token[] compileFormat(LoggerId? id, string format)
    {
        List<Token> cache = [];
        var buffer = new StringBuilder();
        bool isKeyword = false;
        foreach(var c in format)
        {
            if(!isKeyword)
            {
                if(c != '{' && c != '}')
                {
                    buffer.Append(c);
                }
                else {
                    if(buffer.Length > 0) cache.Add(new(TokenId.Text, buffer.ToString()));
                    buffer.Clear();
                    buffer.Append(c);
                    isKeyword = true;
                }
                continue;
            }
            else
            {
                // 공백무시
                if(c == ' ') continue;

                buffer.Append(c);
                if(c != '}' && c != '{') continue;

                string bufferString = buffer.ToString();
                switch(bufferString)
                {
                    // Mecros
                    case Keyword.Date: { cache.Add(new(TokenId.Date)); break; }
                    case Keyword.Time: { cache.Add(new(TokenId.Time)); break; }
                    case Keyword.Level: { cache.Add(new(TokenId.Level, id)); break; }

                    // Message
                    case Keyword.Message: { cache.Add(new(TokenId.Message)); break; }

                    // Colors
                    case Keyword.Color.Red:
                    case Keyword.Color.Yellow:
                    case Keyword.Color.Green:
                    case Keyword.Color.Blue:
                    case Keyword.Color.Gray:
                    case Keyword.Color.Reset:
                        if(bufferString.Equals(Keyword.Color.Reset)) cache.Add(new(TokenId.ColorReset));
                        else if(colorMap.TryGetValue(bufferString, out var consoleColor)) cache.Add(new(TokenId.Color, consoleColor));
                        else throw new ArgumentException($"Unknown Color: {buffer}");
                        break;

                    // 중괄호
                    case "{{": { cache.Add(new(TokenId.Text, "{")); break; }
                    case "}}": { cache.Add(new(TokenId.Text, "}")); break; }

                    // Default
                    default:
                        throw new ArgumentException($"Unknown Token: {buffer}");
                }
                isKeyword = false;
                buffer.Clear();
            }
        }
        if(isKeyword) throw new ArgumentException("Format not Valid");
        if (buffer.Length > 0) cache.Add(new(TokenId.Text, buffer.ToString()));
        return [.. cache];
    }

    private static void setFormat(LoggerId id, string format) {
        if(id == LoggerId.Default) defaultFormat = compileFormat(null, format);
        else formatMap[id] = compileFormat(id, format);
    }

    private static Token[]? getFormat(LoggerId id) => formatMap.GetValueOrDefault(id, null);
}