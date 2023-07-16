using System.Net;
using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static XamarinFiles.PdHelpers.Shared.StatusCodeDetails;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Bundlers
    {
        public static ProblemReport
            BundleProblemReport(DetailsVariant detailsVariant,
                HttpStatusCode statusCodeEnum,
                string title = null,
                string detail = null,
                string instance = null,
                string type = null,
                string[] developerMessages = null,
                string[] userMessages = null,
                ExceptionMessages exceptionMessages = null
        )
        {
            var statusCodeInt = (int)statusCodeEnum;

            var problemReport = BundleProblemReport(detailsVariant,
                statusCodeInt,
                title,
                detail,
                instance,
                type,
                developerMessages,
                userMessages,
                exceptionMessages);

            return problemReport;
        }

        public static ProblemReport
            BundleProblemReport(DetailsVariant detailsVariant,
                int statusCodeInt,
                string title = null,
                string detail = null,
                string instance = null,
                string type = null,
                string[] developerMessages = null,
                string[] userMessages = null,
                ExceptionMessages exceptionMessages = null
            )
        {
            var statusCodeDetails =
                GetHttpStatusDetails(statusCodeInt);

            var problemReport = new ProblemReport
            {
                DetailsVariantEnum = detailsVariant,
                StatusCode = statusCodeDetails.Code,
                Title = title ?? statusCodeDetails.Title,
                Detail = detail,
                Instance = instance,
                Type = type ?? statusCodeDetails.Type,
            };

            if (developerMessages != null
                || userMessages != null
                || exceptionMessages != null)
            {
                problemReport.Messages = new Messages
                {
                    DeveloperMessages = developerMessages,
                    UserMessages = userMessages,
                    // TODO Add flag or always include?
                    ExceptionMessages = exceptionMessages
                };
            }

            // TODO Populate Extensions and OtherErrors from additional data

            return problemReport;
        }
    }
}
