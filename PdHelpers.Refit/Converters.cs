using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using RefitProblemDetails = Refit.ProblemDetails;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Converters
    {
        public static ProblemReport
            ConvertFromProblemDetails(DetailsVariant detailsVariant,
                RefitProblemDetails problemDetails, string httpMethod,
                string[] developerMessages, string[] userMessages,
                ExceptionMessages exceptionMessages)
        {
            var problemReport = BundleProblemReport(detailsVariant,
                problemDetails.Status,
                problemDetails.Title,
                problemDetails.Detail,
                problemDetails.Instance,
                problemDetails.Type,
                httpMethod,
                developerMessages,
                userMessages,
                exceptionMessages);

            return problemReport;
        }
    }
}
