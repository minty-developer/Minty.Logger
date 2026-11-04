using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;

namespace Minty.Logger;

public static partial class Logger
{
    public static LogLevel Level = LogLevel.Info;

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