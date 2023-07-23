using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ExceptionMessages
    {
        public string? ExceptionMessage { get; set; }

        public string? InnerExceptionMessage { get; set; }

        // TODO Break out other exception messages when add logic to pull them
        public IDictionary<string, string> OtherExceptionMessages { get; set; } =
            new Dictionary<string, string>();
    }
}
