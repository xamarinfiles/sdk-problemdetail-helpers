using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    // TODO Add other properties>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class AppStateDetails
    {
        #region Smart Constructor

        private AppStateDetails(string? location, string? operation)
        {
            Location = location;
            Operation = operation;
        }

        public static AppStateDetails?
            Create(string? location = null, string? operation = null)
        {
            if (location is null
                && operation is null)
                return null;

            var appStateDetails = new AppStateDetails(location, operation);

            return appStateDetails;
        }

        #endregion

        #region Properties

        [JsonPropertyName("location")]
        public string? Location { get; }

        [JsonPropertyName("operation")]
        public string? Operation { get; }

        #endregion
    }
}
