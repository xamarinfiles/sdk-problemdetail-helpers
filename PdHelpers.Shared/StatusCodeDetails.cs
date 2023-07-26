using System.Collections.Generic;
using static System.Net.HttpStatusCode;

namespace XamarinFiles.PdHelpers.Shared
{
    // TODO Switch key to HttpStatusCode once off .NET Std 2.0 for XF?
    // Adapted from Microsoft.AspNetCore.Http.Extensions.ProblemDetailsDefaults.Defaults
    // by adding some missing enum entries from System.Net.HttpStatusCode and merging
    // with links to standard codes in IETF RFCs 7231, 7232, 7233, and 7235
    //
    // Non-standard codes included: 422 (RFC 4918 - WebDAV)
    //
    // Non-standard codes skipped: 102, 103, 207, 208, 226, 306, 308, 421, 423,
    // 424, 428, 429, 431, 451, 506, 507, 508, 510, 511

    public static class StatusCodeDetails
    {
        public static (int Code, string Title, string Type)
            GetHttpStatusDetails(int statusCodeInt)
        {
            return HttpStatusDetails.TryGetValue(statusCodeInt,
                out var statusCodeDetails)
                ? statusCodeDetails
                : HttpStatusDetails[(int)InternalServerError];
        }

        private static readonly Dictionary<int, (int Code, string Title, string Type)>
            HttpStatusDetails =
                new Dictionary<int, (int Code, string Title, string Type)>
                {
                    #region Informational 1xx

                    [(int)Continue] =
                    (
                        100,
                        "Continue",
                        "https://tools.ietf.org/html/rfc7231#section-6.2.1"
                    ),

                    [(int)SwitchingProtocols] =
                    (
                        101,
                        "Switching Protocols",
                        "https://tools.ietf.org/html/rfc7231#section-6.2.2"
                    ),

                    #endregion

                    #region Successful 2xx

                    [(int)OK] =
                    (
                        200,
                        "OK",
                        "https://tools.ietf.org/html/rfc7231#section-6.3.1"
                    ),

                    [(int)Created] =
                    (
                        201,
                        "Created",
                        "https://tools.ietf.org/html/rfc7231#section-6.3.2"
                    ),

                    [(int)Accepted] =
                    (
                        202,
                        "Accepted",
                        "https://tools.ietf.org/html/rfc7231#section-6.3.3"
                    ),

                    [(int)NonAuthoritativeInformation] =
                    (
                        203,
                        "Non-Authoritative Information",
                        "https://tools.ietf.org/html/rfc7231#section-6.3.4"
                    ),

                    [(int)NoContent] =
                    (
                        204,
                        "No Content",
                        "https://tools.ietf.org/html/rfc7231#section-6.3.5"
                    ),

                    [(int)ResetContent] =
                    (
                        205,
                        "Reset Content",
                        "https://tools.ietf.org/html/rfc7231#section-6.3.6"
                    ),

                    [(int)PartialContent] =
                    (
                        206,
                        "Partial Content",
                        "https://tools.ietf.org/html/rfc7233#section-4.1"
                    ),

                    #endregion

                    #region Redirection 3xx

                    // Also "Multiple Choices"
                    [(int)Ambiguous] =
                    (
                        300,
                        "Ambiguous",
                        "https://tools.ietf.org/html/rfc7231#section-6.4.1"
                    ),

                    // Also "Moved Permanently"
                    [(int)Moved] =
                    (
                        301,
                        "Moved",
                        "https://tools.ietf.org/html/rfc7231#section-6.4.2"
                    ),

                    // Also "Found"
                    [(int)Redirect] =
                    (
                        302,
                        "Redirect",
                        "https://tools.ietf.org/html/rfc7231#section-6.4.3"
                    ),

                    // Also "See Other"
                    [(int)RedirectMethod] =
                    (
                        303,
                        "Redirect Method",
                        "https://tools.ietf.org/html/rfc7231#section-6.4.4"
                    ),

                    [(int)NotModified] =
                    (
                        304,
                        "NotModified",
                        "https://tools.ietf.org/html/rfc7232#section-4.1"
                    ),

                    [(int)UseProxy] =
                    (
                        305,
                        "Use Proxy",
                        "https://tools.ietf.org/html/rfc7231#section-6.4.5"
                    ),

                    // Also "Redirect Keep Verb"
                    [(int)TemporaryRedirect] =
                    (
                        307,
                        "Temporary Redirect",
                        "https://tools.ietf.org/html/rfc7231#section-6.4.7"
                    ),

                    #endregion

                    #region Client Error 4xx

                    [(int)BadRequest] =
                    (
                        400,
                        "Bad Request",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    ),

                    [(int)Unauthorized] =
                    (
                        401,
                        "Unauthorized",
                        "https://tools.ietf.org/html/rfc7235#section-3.1"
                    ),

                    [(int)PaymentRequired] =
                    (
                        402,
                        "Payment Required",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.2"
                    ),

                    [(int)Forbidden] =
                    (
                        403,
                        "Forbidden",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.3"
                    ),

                    [(int)NotFound] =
                    (
                        404,
                        "Not Found",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.4"
                    ),

                    [(int)MethodNotAllowed] =
                    (
                        405,
                        "Method Not Allowed",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.5"
                    ),

                    [(int)NotAcceptable] =
                    (
                        406,
                        "Not Acceptable",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.6"
                    ),

                    [(int)ProxyAuthenticationRequired] =
                    (
                        407,
                        "Proxy Authentication Required",
                        "https://tools.ietf.org/html/rfc7235#section-3.2"
                    ),

                    [(int)RequestTimeout] =
                    (
                        408,
                        "Request Timeout",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.7"
                    ),

                    [(int)Conflict] =
                    (
                        409,
                        "Conflict",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.8"
                    ),

                    [(int)Gone] =
                    (
                        410,
                        "Gone",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.9"
                    ),

                    [(int)LengthRequired] =
                    (
                        411,
                        "Length Required",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.10"
                    ),

                    [(int)PreconditionFailed] =
                    (
                        412,
                        "Precondition Failed",
                        "https://tools.ietf.org/html/rfc7232#section-4.2"
                    ),

                    // Also "Payload Too Large"
                    [(int)RequestEntityTooLarge] =
                    (
                        413,
                        "Request Entity Too Large",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.11"
                    ),

                    // Also "Uri Too Long"
                    [(int)RequestUriTooLong] =
                    (
                        414,
                        "Request Uri Too Long",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.12"
                    ),

                    [(int)UnsupportedMediaType] =
                    (
                        415,
                        "Unsupported Media Type",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.13"
                    ),

                    // Also "Range Not Satisfiable"
                    [(int)RequestedRangeNotSatisfiable] =
                    (
                        416,
                        "Requested Range Not Satisfiable",
                        "https://tools.ietf.org/html/rfc7233#section-4.4"
                    ),

                    [(int)ExpectationFailed] =
                    (
                        417,
                        "Expectation Failed",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.14"
                    ),

#if NETSTANDARD2_0
                    // UnprocessableEntity (422) was not added until .NET Core 2.1
                    [422]
#else
                    [(int)UnprocessableEntity]
#endif
                    =
                    (
                        422,
                        "Unprocessable Entity",
                        "https://tools.ietf.org/html/rfc4918#section-11.2"
                    ),

                    [(int)UpgradeRequired] =
                    (
                        426,
                        "Upgrade Required",
                        "https://tools.ietf.org/html/rfc7231#section-6.5.15"
                    ),

                    #endregion

                    #region Server Error 5xx

                    [(int)InternalServerError] =
                    (
                        500,
                        "Internal Server Error",
                        "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                    ),

                    [(int)NotImplemented] =
                    (
                        501,
                        "Not Implemented",
                        "https://tools.ietf.org/html/rfc7231#section-6.6.2"
                    ),

                    [(int)BadGateway] =
                    (
                        502,
                        "Bad Gateway",
                        "https://tools.ietf.org/html/rfc7231#section-6.6.3"
                    ),

                    [(int)ServiceUnavailable] =
                    (
                        503,
                        "Service Unavailable",
                        "https://tools.ietf.org/html/rfc7231#section-6.6.4"
                    ),

                    [(int)GatewayTimeout] =
                    (
                        504,
                        "GatewayTimeout",
                        "https://tools.ietf.org/html/rfc7231#section-6.6.5"
                    ),

                    [(int)HttpVersionNotSupported] =
                    (
                        505,
                        "Http Version Not Supported",
                        "https://tools.ietf.org/html/rfc7231#section-6.6.6"
                    )

                    #endregion
                };
    }
}
