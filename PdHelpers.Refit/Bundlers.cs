using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.Net.HttpStatusCode;
using static XamarinFiles.PdHelpers.Refit.Enums.DetailsVariant;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Bundlers
    {
        internal static ProblemReport
            CreateGenericProblemReport(AppStateDetails? appStateDetails,
                string[]? developerMessages,
                string[]? userMessages,
                ExceptionMessages? exceptionMessages)
        {
            var problemReport =
                ProblemReport.Create(InternalServerError,
                    GenericProblem, ErrorOrWarning.Error,
                    appStateDetails: appStateDetails,
                    developerMessages: developerMessages,
                    userMessages: userMessages,
                    exceptionMessages: exceptionMessages);

            return problemReport;
        }
    }
}
