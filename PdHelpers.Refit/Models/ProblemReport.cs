using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using XamarinFiles.PdHelpers.Refit.Enums;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ProblemReport
    {
        #region Source Format (ProblemDetails or ValidationProblemDetails)

        [JsonIgnore]
        public DetailsVariant DetailsVariantEnum { get; set; }

        [JsonPropertyName("detailsVariant")]
        public string DetailsVariantName =>
            DetailsVariantEnum == DetailsVariant.ValidationProblem
                ? "ValidationProblemDetails"
                : "ProblemDetails";

        #endregion

        #region Common Fields (IETF RFC 7807)

        [JsonPropertyName("status")]
        public int StatusCode { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("detail")]
        public string? Detail { get; set; }

        [JsonPropertyName("instance")]
        public string? Instance { get; set; }

        // TODO Keep default value from Refit?
        [JsonPropertyName("type")]
        public string Type { get; set; } = "about:blank";

        #endregion

        #region Other Context Information

        public string? Method { get; set; }

        #endregion

        #region Expected Messages Promoted from Extensions/Errors

        [JsonPropertyName("messages")]
        public Messages? Messages { get; set; }

        #endregion

        #region Remaining Extensions From MVC and Refit ProblemDetails

        [JsonExtensionData]
        [JsonPropertyName("extensions")]
        public IDictionary<string, object> Extensions { get; set; } =
            new Dictionary<string, object>();

        #endregion

        #region Remaining Errors From MVC ValidationDetails / Refit ProblemDetails

        [JsonPropertyName("otherErrors")]
        public Dictionary<string, string[]> OtherErrors { get; set; } =
            new();

        #endregion
    }
}
