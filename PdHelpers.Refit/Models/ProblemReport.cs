using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
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

        private ProblemReport(DetailsVariant detailsVariant,
            ErrorOrWarning errorOrWarning,
            SourceDetails? sourceDetails,
            RequestDetails? requestDetails,
            ResponseDetails responseDetails,
            Messages? importantMessages)
        {
            DetailsVariantEnum = detailsVariant;
            ErrorOrWarningEnum = errorOrWarning;
            SourceDetails = sourceDetails;
            RequestDetails = requestDetails;
            ResponseDetails = responseDetails;
            ImportantMessages = importantMessages;
        }

        // StatusCode integer variant [current until after .NET Std 2.0 + XF]
        public static ProblemReport
            Create(int statusCodeInt,
                DetailsVariant detailsVariant,
                ErrorOrWarning errorOrWarning,
                SourceDetails? sourceDetails = null,
                HttpRequestMessage? requestMessage = null,
                string? resourceName = null,
                string? title = null,
                string? detail = null,
                string? instance = null,
                string[]? developerMessages = null,
                string[]? userMessages = null,
                ExceptionMessages? exceptionMessages = null)
        {
            var requestDetails =
                RequestDetails.Create(requestMessage, resourceName);

            var responseDetails =
                ResponseDetails.Create(statusCodeInt, instanceUri: instance,
                    problemSummary: title, problemExplanation: detail);

            var importantMessages =
                Messages.Create(developerMessages, userMessages,
                    exceptionMessages);

            var problemReport =
                new ProblemReport(detailsVariant, errorOrWarning,
                    sourceDetails, requestDetails, responseDetails,
                    importantMessages);

            return problemReport;
        }

        // StatusCode enum variant [alternative / future after .NET Std 2.0 + XF]
        public static ProblemReport
            Create(HttpStatusCode statusCode,
                DetailsVariant detailsVariant,
                ErrorOrWarning errorOrWarning,
                SourceDetails? sourceDetails = null,
                HttpRequestMessage? requestMessage = null,
                string? resourceName = null,
                string? title = null,
                string? detail = null,
                string? instance = null,
                string[]? developerMessages = null,
                string[]? userMessages = null,
                ExceptionMessages? exceptionMessages = null)
        {
            var statusCodeInt = (int)statusCode;

            var problemReport =
                Create(statusCodeInt, detailsVariant, errorOrWarning,
                    sourceDetails, requestMessage, resourceName, title, detail,
                    instance, developerMessages, userMessages, exceptionMessages);

            return problemReport;
        }

        #endregion

        #region Source Format (ProblemDetails or ValidationProblemDetails)

        [JsonIgnore]
        public DetailsVariant DetailsVariantEnum { get; }

        [JsonPropertyName("detailsVariant")]
        public string
            DetailsVariantName =>
                DetailsVariantEnum == DetailsVariant.ValidationProblem
                    ? "ValidationProblemDetails"
                    : "ProblemDetails";

        #endregion

        #region Source Condition (Error or Warning)

        [JsonIgnore]
        public ErrorOrWarning ErrorOrWarningEnum { get; }

        [JsonPropertyName("errorOrWarning")]
        public string ErrorOrWarning => ErrorOrWarningEnum.ToString();

        #endregion

        #region App State Details

        [JsonPropertyName("sourceDetails")]
        public SourceDetails? SourceDetails { get; }

        #endregion

        #region HttpRequestMessage fields

        [JsonPropertyName("requestDetails")]
        public RequestDetails? RequestDetails { get; }

        #endregion

        #region ProblemDetails Response Fields (IETF RFC 7807)

        [JsonPropertyName("responseDetails")]
        public ResponseDetails ResponseDetails { get; }

        #endregion

        #region Expected Messages Promoted from Extensions/Errors

        [JsonPropertyName("messages")]
        public Messages? ImportantMessages { get; }

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
