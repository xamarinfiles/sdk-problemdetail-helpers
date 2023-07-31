using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    // Human-readable names for code segments (vs compiler names from exception)
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class SourceDetails
    {
        #region Smart Constructor

        private SourceDetails(string? assemblyName,
            string? componentName,
            string? operationName)
        {
            AssemblyName = assemblyName;
            ComponentName = componentName;
            OperationName = operationName;
        }

        public static SourceDetails?
            Create(string? assemblyName = null,
                string? componentName = null,
                string? operationName = null)
        {
            if (assemblyName is null
                && componentName is null
                && operationName is null)
                return null;

            var sourceDetails =
                new SourceDetails(assemblyName, componentName,
                    operationName);

            return sourceDetails;
        }

        #endregion

        #region User-supplied Properties

        [JsonPropertyName("assemblyName")]
        public string? AssemblyName { get; }

        [JsonPropertyName("componentName")]
        public string? ComponentName { get; }

        [JsonPropertyName("operationName")]
        public string? OperationName { get; }

        #endregion
    }
}
