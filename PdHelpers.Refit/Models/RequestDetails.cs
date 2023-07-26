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
            string? resourceName)
        {
            HttpMethod = requestMessage?.Method;
            Uri = requestMessage?.RequestUri;
            ResourceName = resourceName;
        }

        public static RequestDetails?
            Create(HttpRequestMessage? requestMessage = null,
            string? resourceName = null)
        {
            if (requestMessage is null
                && resourceName is null)
                return null;

            var requestDetails =
                new RequestDetails(requestMessage, resourceName);

            return requestDetails;
        }

        #endregion

        #region Properties

        [JsonIgnore]
        public HttpMethod? HttpMethod { get; }

        [JsonPropertyName("requestHttpMethod")]
        public string? HttpMethodName => HttpMethod?.Method;

        [JsonPropertyName("resourceName")]
        public string? ResourceName { get; }

        [JsonIgnore]
        public Uri? Uri { get; }

        [JsonPropertyName("requestUri")]
        public string? UriString => Uri?.ToString();

        #endregion
    }
}
