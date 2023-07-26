using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.Net.HttpStatusCode;
using static XamarinFiles.PdHelpers.Refit.Enums.DetailsVariant;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Bundlers
    {
        internal static ProblemReport
            CreateGenericProblemReport(
                string? sourceName,
                string? sourceLocation,
                string? sourceOperation,
                string[]? developerMessages,
                string[]? userMessages,
                ExceptionMessages? exceptionMessages)
        {
            var sourceDetails =
                SourceDetails.Create(sourceName, sourceLocation, sourceOperation);

            var problemReport =
                ProblemReport.Create(InternalServerError,
                    GenericProblem, ErrorOrWarning.Error,
                    sourceDetails: sourceDetails,
                    developerMessages: developerMessages,
                    userMessages: userMessages,
                    exceptionMessages: exceptionMessages);

            return problemReport;
        }
    }
}
