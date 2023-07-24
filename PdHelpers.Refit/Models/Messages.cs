using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Messages
    {
        [JsonPropertyName("developerMessages")]
        public IEnumerable<string>? DeveloperMessages { get; set; }
            = new List<string>();

        [JsonPropertyName("exceptionMessages")]
        public ExceptionMessages? ExceptionMessages { get; set; }

        [JsonPropertyName("userMessages")]
        public IEnumerable<string>? UserMessages { get; set; }
            = new List<string>();
    }
}
