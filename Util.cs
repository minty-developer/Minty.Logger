using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Minty.Logger;

/*
git tag v0.x.x
git push origin v0.x.x
*/

/*
# 그리고 앞으로 만들면 좋을 기능들

✅ Console 출력
✅ Warning 로그
✅ Debug 로그
✅ http 로그
✅ 로그 레벨 필터 (Logger.Level = LogLevel.Warning)
⏳ 파일 저장
✅ Formatter 교체
✅ HTTP 요청 전용 로그
⏳ RequestId 자동 생성
⏳ 비동기 파일 기록
⏳ JSON 로그 출력
*/

public static partial class Logger
{
    private static void info(string context)
    {
        if(Level > LogLevel.Info) return;
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("[Info]");
        Console.ResetColor();
        Console.Write("    | ");
        Console.WriteLine(context);
    }
    private static void success(string context)
    {
        if(Level > LogLevel.Info) return;
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[Success]");
        Console.ResetColor();
        Console.Write(" | ");
        Console.WriteLine(context);
    }
    private static void debug<T>(string name, T value)
    {
        if(Level > LogLevel.Debug) return;
        Console.WriteLine($"                          [Debug]   | {name}: {value}");
    }
    private static void warn(string? warnCode, string context)
    {
        if(Level> LogLevel.Warn) return;
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[Warn]");
        Console.ResetColor();
        Console.Write("    | ");
        if(warnCode is not null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{warnCode}");
            Console.ResetColor();
            Console.Write(" | ");
        }
        Console.WriteLine($"{context}");
    }
    private static void error(string? errorCode, string? context, Exception? error)
    {
        if(Level > LogLevel.Error) return;
        Console.WriteLine("========================================================================");
        Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($" [Error] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"  | ");
        if(errorCode is not null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{errorCode}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" | ");
        }
        if(context is not null) {
            Console.WriteLine($"{context}");
        } else if(error is not null)
        {
                    Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(error.GetType().Name);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" | {error.Message}");
            if (error.StackTrace != null) Console.WriteLine($"{error.StackTrace.Split(Environment.NewLine)[0]}");
        } else
        {
            throw new ArgumentException("인자가 잘못 전달되었습니다.");
        }
        Console.WriteLine("========================================================================");
        Console.ResetColor();
    }
    public static partial class Http
    {
        private const string StopwatchKey = "__MintyLoggerStopwatch";
        private static void request(HttpContext context)
        {
            if(Level > LogLevel.Info) return;
            context.Items[StopwatchKey] = Stopwatch.StartNew();
            HttpRequest request = context.Request;
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {request.Scheme} {request.Method} {request.Path}{request.QueryString} from {request.Host}");
        }
        private static void response(HttpContext context)
        {
            if(Level > LogLevel.Info) return;
            HttpResponse response = context.Response;
            long elapsed = 0;
            if (context.Items[StopwatchKey] is Stopwatch sw)
            {
                sw.Stop();
                elapsed = sw.ElapsedMilliseconds;
            }
            Console.Write($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] Response end with ");
            Console.ForegroundColor = ConsoleColor.Green;
            if(response.StatusCode >= 300) Console.ForegroundColor = ConsoleColor.Yellow;
            if(response.StatusCode >= 400) Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{response.StatusCode}");
            Console.ResetColor();
            Console.WriteLine($" in {elapsed}ms ({response.ContentLength}b)");
        }
        private static void httpsStart(HttpContext context)
        {
            if(Level > LogLevel.Info) return;
            context.Items[StopwatchKey] = Stopwatch.StartNew();
        }
        private static void httpsEnd(HttpContext context)
        {
            if(Level > LogLevel.Info) return;
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            Stopwatch? sw = context.Items[StopwatchKey] as Stopwatch;
            double elapsed = 0;
            if (sw != null)
            {
                sw.Stop();
                elapsed = sw.Elapsed.TotalMilliseconds;
            }
            Console.WriteLine("========================================================================");
            Console.WriteLine("  [HTTP]");
            Console.WriteLine($"TRACE   | {context.TraceIdentifier}");
            Console.Write("STATUS  | ");
            Console.ForegroundColor = ConsoleColor.Green;
            if(response.StatusCode >= 300) Console.ForegroundColor = ConsoleColor.Yellow;
            if(response.StatusCode >= 400) Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{response.StatusCode}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"METHOD  | {request.Scheme} {request.Method}");
            Console.WriteLine($"TYPE    | {request.ContentType ?? "-"}");
            Console.WriteLine($"FROM    | {request.Host}");
            Console.WriteLine($"TO      | {request.Path}{request.QueryString}");
            Console.WriteLine("");
            Console.WriteLine($"ELAPSED | {elapsed:F2} ms");
            Console.WriteLine($"SIZE    | {response.ContentLength ?? 0}b");
            Console.WriteLine("");
            Console.WriteLine($"PROTOC  | {request.Protocol}");
            Console.WriteLine($"AGENT   | {request.Headers.UserAgent.FirstOrDefault() ?? "-"}");
            Console.WriteLine($"CLIENT  | {context.Connection.RemoteIpAddress?.ToString() ?? "-"}");
            Console.WriteLine("========================================================================");
        }
    }
}