using Minty.Logger.Util;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;

namespace Minty.Logger.Log;

/*
# 그리고 앞으로 만들면 좋을 기능들

✅ Console 출력
✅ Warning 로그
✅ Debug 로그
✅ http 로그
⏳ 로그 레벨 필터 (Logger.Level = LogLevel.Warning)
⏳ 파일 저장
⏳ Formatter 교체
⏳ HTTP 요청 전용 로그
⏳ RequestId 자동 생성
⏳ 비동기 파일 기록
⏳ JSON 로그 출력
*/

public static class Logger
{
    public static void info(string context) => Print.Info(context);
    public static void success(string context) => Print.Success(context);
    public static void debug<T>(T value, [CallerArgumentExpression("value")] string? name = null) => Print.Debug(name?? "", value);
    public static void warn(string context) => Print.Warn(context);
    public static void warn(string warncode, string context) => Print.Warn(warncode, context);
    public static void error(string context) => Print.Error(context);
    public static void error(string errorcode, string context) => Print.Error(errorcode, context);
    public static void error(Exception Error) => Print.Error(Error);
    public static class Http
    {
        public static void reqIn(HttpContext context) => Print.Http.reqIn(context);
        public static void resOut(HttpContext context) => Print.Http.resOut(context);
        public static void startHttpReqRes(HttpContext context) => Print.Http.startHttpReqRes(context);
        public static void httpReqRes(HttpContext context) => Print.Http.httpReqRes(context);
    }
}