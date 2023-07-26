using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    // TODO Add other properties like StackTrace?
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class SourceDetails
    {
        #region Smart Constructor

        private SourceDetails(string? sourceName,
            string? sourceLocation,
            string? sourceOperation)
        {
            Name = sourceName;
            Location = sourceLocation;
            Operation = sourceOperation;
        }

        public static SourceDetails?
            Create(string? sourceName = null,
                string? sourceLocation = null,
                string? sourceOperation = null)
        {
            if (sourceName is null
                && sourceLocation is null
                && sourceOperation is null)
                return null;

            var sourceDetails =
                new SourceDetails(sourceName, sourceLocation, sourceOperation);

            return sourceDetails;
        }

        public static SourceDetails?
            Create(Exception exception,
                string? sourceOperation = null)
        {
            var sourceDetails =
                new SourceDetails(exception.Source,
                    exception.TargetSite.Name,
                    sourceOperation);

            return sourceDetails;
        }

        #endregion

        #region Exception Properties

        [JsonPropertyName("name")]
        public string? Name { get; }

        [JsonPropertyName("location")]
        public string? Location { get; }

        #endregion

        #region User-supplied Properties

        [JsonPropertyName("operation")]
        public string? Operation { get; }

        #endregion
    }
}
