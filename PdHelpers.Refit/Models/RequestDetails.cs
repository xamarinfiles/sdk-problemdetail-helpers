using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    // TODO Pull API request details:
    // * JsonContent with toggle?
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class RequestDetails
    {
        #region Smart Constructor

        private RequestDetails(HttpRequestMessage? requestMessage,
            string? controllerName,
            string? resourceName)
        {
            HttpMethod = requestMessage?.Method;
            Uri = requestMessage?.RequestUri;

            Controller = controllerName;
            Resource = resourceName;
        }

        public static RequestDetails?
            Create(HttpRequestMessage? requestMessage = null,
                string? controllerName = null,
                string? resourceName = null)
        {
            if (requestMessage is null
                && controllerName is null
                && resourceName is null)
                return null;

            var requestDetails =
                new RequestDetails(requestMessage, controllerName, resourceName);

            return requestDetails;
        }

        #endregion

        #region HttpRequestMessage Properties

        [JsonIgnore]
        internal HttpMethod? HttpMethod { get; }

        [JsonPropertyName("method")]
        public string? Method => HttpMethod?.Method;

        [JsonPropertyName("uri")]
        public Uri? Uri { get; }

        //[JsonPropertyName("uri")]
        //public string? UriStr => Uri?.ToString();

        #endregion

        #region Passed Properties

        [JsonPropertyName("controller")]
        public string? Controller { get; }

        [JsonPropertyName("resource")]
        public string? Resource { get; }

        #endregion
    }
}
