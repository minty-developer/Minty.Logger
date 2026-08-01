using Microsoft.AspNetCore.Http;
using System.Net.Cache;
using System.Net.Mime;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace Minty.Logger.Util;

public static class Print
{
    public static void Info(string context)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("[Info]");
        Console.ResetColor();
        Console.Write("    | ");
        Console.WriteLine(context);
    }
    public static void Success(string context)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[Success]");
        Console.ResetColor();
        Console.Write(" | ");
        Console.WriteLine(context);
    }
    public static void Debug<T>(string name, T value)
    {
        Console.WriteLine($"                          [Debug]   | {name}: {value}");
    }
    public static void Warn(string context)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[Warn]");
        Console.ResetColor();
        Console.WriteLine($"    | {context}");
    }
    public static void Warn(string warnCode, string context)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[Warn]");
        Console.ResetColor();
        Console.Write($"    | ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{warnCode}");
        Console.ResetColor();
        Console.Write(" | ");
        Console.WriteLine(context);
    }
    public static void Error(string context)
    {
        Console.WriteLine("========================================================================");
        Console.Write($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($" [Error] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"  | {context}");
        Console.WriteLine("========================================================================");
        Console.ResetColor();
    }
    public static void Error(string errorCode, string context)
    {
        Console.WriteLine("========================================================================");
        Console.Write($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($" [Error] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"  | ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{errorCode}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" | {context}");
        Console.WriteLine("========================================================================");
        Console.ResetColor();
    }
    public static void Error(Exception error)
    {
        Console.WriteLine("========================================================================");
        Console.Write($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($" [Error] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"  | ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(error.GetType().Name);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" | {error.Message}");
        if (error.StackTrace != null) Console.WriteLine($"{error.StackTrace.Split(Environment.NewLine)[0]}");
        Console.WriteLine("========================================================================");
        Console.ResetColor();
    }
    public static class Http
    {
        private const string StopwatchKey = "__MintyLoggerStopwatch";
        public static void reqIn(HttpContext context)
        {
            context.Items[StopwatchKey] = Stopwatch.StartNew();
            HttpRequest request = context.Request;
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] {request.Scheme} {request.Method} {request.Path}{request.QueryString} from {request.Host}");
        }
        public static void resOut(HttpContext context)
        {
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
        public static void startHttpReqRes(HttpContext context)
        {
            context.Items[StopwatchKey] = Stopwatch.StartNew();
        }
        public static void httpReqRes(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            Stopwatch? sw = context.Items[StopwatchKey] as Stopwatch;
            double elapsed = 0;
            if (sw != null)
            {
                sw.Stop();
                elapsed = sw.Elapsed.TotalMilliseconds;
            }
            Console.WriteLine($"ELAPSED | {elapsed:F2} ms");
            Console.WriteLine("========================================================================");
            Console.WriteLine("  [HTTP]");
            Console.WriteLine($"TRACE   | {context.TraceIdentifier}");
            Console.Write("STATUS  | ");
            Console.ForegroundColor = ConsoleColor.Green;
            if(response.StatusCode >= 300) Console.ForegroundColor = ConsoleColor.Yellow;
            if(response.StatusCode >= 400) Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{response.StatusCode}");
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