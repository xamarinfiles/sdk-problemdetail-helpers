using Refit;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using XamarinFiles.PdHelpers.Refit.Enums;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ProblemReport
    {
        #region Smart Constructor

        private ProblemReport(ProblemVariant problemVariant,
            ProblemLevel problemLevel,
            SourceDetails? sourceDetails,
            ExceptionDetails? exceptionDetails,
            RequestDetails? requestDetails,
            ResponseDetails responseDetails,
            OtherMessages? otherMessages)
        {
            ProblemVariantEnum = problemVariant;
            ProblemLevelEnum = problemLevel;
            SourceDetails = sourceDetails;
            ExceptionDetails = exceptionDetails;
            RequestDetails = requestDetails;
            ResponseDetails = responseDetails;
            OtherMessages = otherMessages;
        }

        // StatusCode integer variant [current until after .NET Std 2.0 + XF]
        public static ProblemReport
            Create(int statusCodeInt,
                ProblemVariant problemVariant,
                ProblemLevel problemLevel,
                // SourceDetails
                string? assemblyName = null,
                string? componentName = null,
                string? operationName = null,
                // ExceptionDetails / RequestDetails
                ApiException? apiException = null,
                string? controllerName = null,
                string? resourceName = null,
                // ResponseDetails
                string? title = null,
                string? detail = null,
                string? instance = null,
                // OtherMessages
                string[]? developerMessages = null,
                string[]? userMessages = null)
        {
            var sourceDetails =
                SourceDetails.Create(assemblyName, componentName,
                    operationName);

            var exceptionDetails = ExceptionDetails.Create(apiException);

            var requestDetails =
                RequestDetails.Create(apiException?.RequestMessage,
                    controllerName, resourceName);

            var responseDetails =
                ResponseDetails.Create(statusCodeInt, instanceUri: instance,
                    problemSummary: title, problemExplanation: detail);

            var otherMessages =
                OtherMessages.Create(developerMessages?.ToList(),
                    userMessages?.ToList());

            var problemReport =
                new ProblemReport(problemVariant, problemLevel,
                    sourceDetails, exceptionDetails, requestDetails,
                    responseDetails, otherMessages);

            return problemReport;
        }

        // StatusCode enum variant [alternative / future after .NET Std 2.0 + XF]
        public static ProblemReport
            Create(HttpStatusCode statusCode,
                ProblemVariant problemVariant,
                ProblemLevel problemLevel,
                string? assemblyName = null,
                string? componentName = null,
                string? operationName = null,
                ApiException? apiException = null,
                string? controllerName = null,
                string? resourceName = null,
                string? title = null,
                string? detail = null,
                string? instance = null,
                string[]? developerMessages = null,
                string[]? userMessages = null)
        {
            var statusCodeInt = (int)statusCode;

            var problemReport =
                Create(statusCodeInt, problemVariant, problemLevel,
                    assemblyName, componentName, operationName, apiException,
                    controllerName, resourceName, title, detail, instance,
                    developerMessages, userMessages);

            return problemReport;
        }

        #endregion

        #region Source Format (ProblemDetails or ValidationProblemDetails)

        [JsonIgnore]
        public ProblemVariant ProblemVariantEnum { get; }

        [JsonPropertyName("problemVariant")]
        public string ProblemVariant => ProblemVariantEnum.ToString();

        #endregion

        #region Source Condition (Error or Warning)

        [JsonIgnore]
        public ProblemLevel ProblemLevelEnum { get; }

        [JsonPropertyName("problemLevel")]
        public string ProblemLevel => ProblemLevelEnum.ToString();

        #endregion

        #region App State Details

        [JsonPropertyName("source")]
        public SourceDetails? SourceDetails { get; }

        #endregion

        #region Exception Details

        [JsonPropertyName("exception")]
        public ExceptionDetails? ExceptionDetails { get; }

        #endregion

        #region HttpRequestMessage fields

        [JsonPropertyName("request")]
        public RequestDetails? RequestDetails { get; }

        #endregion

        #region ProblemDetails Response Fields (IETF RFC 7807)

        [JsonPropertyName("response")]
        public ResponseDetails ResponseDetails { get; }

        #endregion

        #region Expected Messages Promoted from Extensions/Errors

        [JsonPropertyName("messages")]
        public OtherMessages? OtherMessages { get; }

        #endregion

        // TODO
        //#region Remaining Extensions From MVC and Refit ProblemDetails

        //[JsonExtensionData]
        //[JsonPropertyName("extensions")]
        //public IDictionary<string, object> Extensions { get; } =
        //    new Dictionary<string, object>();

        //#endregion

        // TODO
        //#region Remaining Errors From MVC ValidationDetails / Refit ProblemDetails

        //[JsonPropertyName("otherErrors")]
        //public Dictionary<string, string[]> OtherErrors { get; } =
        //    new();

        //#endregion
    }
}
