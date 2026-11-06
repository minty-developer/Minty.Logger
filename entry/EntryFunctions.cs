using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;

namespace Minty.Log;

public static partial class Logger
{
    public static void SetFormat(LoggerId loggerId, string format) => setFormat(loggerId, format);

    public static void SetLogFilter(LogLevel minLevel) => level = minLevel;

    public static void Export(string path) => export(path);
    public static async Task ExportAsync(string path) => await exportAsync(path);

    public static void AutoExport(string path) => autoExport(path);
    public static async Task AutoExportAsync(string path) => await autoExportAsync(path);

    public static void Debug<T>(T value, [CallerArgumentExpression(nameof(value))] string? name = null) => debug(name?? "", value);

    public static void Info(string context) => info(context);

    public static void Success(string context) => success(context);

    public static void Warn(string context) => warn(null, context);
    public static void Warn(string warncode, string context) => warn(warncode, context);

    public static void Error(string context) => error(null, context, null);
    public static void Error(string errorcode, string context) => error(errorcode, context, null);
    public static void Error(Exception Error) => error(null, null, Error);

    public static partial class Http
    {
        public static void Request(HttpContext context) => request(context);
        public static void Response(HttpContext context) => response(context);
        public static void HttpRequired(HttpContext context) => httpsStart(context);
        public static void HttpRespond(HttpContext context) => httpsEnd(context);
    }
}