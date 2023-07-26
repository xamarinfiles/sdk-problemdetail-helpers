using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using static XamarinFiles.PdHelpers.Shared.StatusCodeDetails;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    // TODO Change SpecificUri from string? to Uri? (Warning CA1054)
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ResponseDetails
    {
        #region Smart Constructor

        private ResponseDetails(int statusCodeInt,
            string statusTitle,
            string statusReference,
            string? instanceUri,
            string? problemSummary,
            string? problemExplanation
        )
        {
            StatusCodeInt = statusCodeInt;
            StatusTitle = statusTitle;
            StatusReference = statusReference;
            InstanceUri = instanceUri;
            ProblemSummary = problemSummary;
            ProblemExplanation = problemExplanation;
        }

        public static ResponseDetails Create(
            int statusCodeInt,
            string? instanceUri = null,
            string? problemSummary = null,
            string? problemExplanation = null)
        {
            var (_, statusTitle, statusReference) =
                GetHttpStatusDetails(statusCodeInt);

            var coreDetails =
                new ResponseDetails(statusCodeInt, statusTitle, statusReference,
                    instanceUri, problemSummary, problemExplanation);

            return coreDetails;
        }

        #endregion

        #region Correspond to ProblemDetails Fields (IETF RFC 7807)

        // IETF RFC 7807: Status
        [JsonPropertyName("statusCodeInt")]
        public int StatusCodeInt { get; }

        // IETF RFC 7807: Type
        [JsonPropertyName("statusReference")]
        public string StatusReference { get; }

        // IETF RFC 7807: Instance
        [JsonPropertyName("instanceUri")]
        public string? InstanceUri { get; }

        // IETF RFC 7807: Title
        [JsonPropertyName("problemSummary")]
        public string? ProblemSummary { get; }

        // IETF RFC 7807: Detail
        [JsonPropertyName("problemExplanation")]
        public string? ProblemExplanation { get; }

        #endregion

        #region Context Fields

        // From System.Net.HttpStatusCode enum value
        [JsonPropertyName("statusTitle")]
        public string StatusTitle { get; }

        #endregion
    }
}
