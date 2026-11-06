using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Minty.Log;

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
    private static LogLevel level = LogLevel.Info;
    private static void info(string context)
    {
        if(level > LogLevel.Info) return;
        logWithFormat(LoggerId.Info, context);
    }
    private static void success(string context)
    {
        if(level > LogLevel.Info) return;
        logWithFormat(LoggerId.Success, context);
    }
    private static void debug<T>(string name, T value)
    {
        if(level > LogLevel.Debug) return;
        logWithFormat(LoggerId.Debug, $"{name}: {value}");
    }
    private static void warn(string? warnCode, string context)
    {
        if(level> LogLevel.Warn) return;
        logWithFormat(LoggerId.Warn, warnCode?.ToString() + ((warnCode is not null)? " ": "") + context);
    }
    private static void error(string? errorCode, string? context, Exception? error)
    {
        if(level > LogLevel.Error) return;
        logWithFormat(LoggerId.Error, error?.Message + ((errorCode is not null && error is not null)? " ": "") + context?.ToString());
        if (error?.StackTrace != null) Console.WriteLine($"{error.StackTrace.Split(Environment.NewLine)[0]}");
    }
    public static partial class Http
    {
        private const string StopwatchKey = "__MintyLoggerStopwatch";
        private static void request(HttpContext context)
        {
            if(level > LogLevel.Info) return;
            context.Items[StopwatchKey] = Stopwatch.StartNew();
            HttpRequest request = context.Request;
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {request.Scheme} {request.Method} {request.Path}{request.QueryString} from {request.Host}");
        }
        private static void response(HttpContext context)
        {
            if(level > LogLevel.Info) return;
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
            if(level > LogLevel.Info) return;
            context.Items[StopwatchKey] = Stopwatch.StartNew();
        }
        private static void httpsEnd(HttpContext context)
        {
            if(level > LogLevel.Info) return;
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