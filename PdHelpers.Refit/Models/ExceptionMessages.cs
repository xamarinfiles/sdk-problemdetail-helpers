using System.Collections.Generic;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    public class ExceptionMessages
    {
        public string ExceptionMessage { get; set; }

        public string InnerExceptionMessage { get; set; }

        // TODO Break out other exception messages when add logic to pull them
        public IDictionary<string, string> OtherExceptionMessages { get; set; }
    }
}
