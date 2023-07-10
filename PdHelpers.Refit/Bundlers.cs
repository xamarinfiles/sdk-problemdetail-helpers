using System.Net;
using static XamarinFiles.PdHelpers.Shared.StatusCodeDetails;
using RefitProblemDetails = Refit.ProblemDetails;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Bundlers
    {
        public static RefitProblemDetails
            BundleRefitProblemDetails(
            HttpStatusCode statusCode,
            string title = null,
            string detail = null,
            string instance = null,
            string[] exceptionMessages = null,
            string[] developerMessages = null,
            string[] userMessages = null
        )
        {
            var statusCodeInt = (int)statusCode;
            var statusCodeDetails =
                GetHttpStatusDetails(statusCodeInt);

            var problemDetails = new RefitProblemDetails
            {
                Status = statusCodeDetails.Code,
                Title = title ?? statusCodeDetails.Title,
                Type = statusCodeDetails.Type,
                Detail = detail,
                Instance = instance
            };

            if (exceptionMessages?.Length > 0)
                problemDetails.Errors.Add("exceptionMessages",
                    exceptionMessages);

            if (developerMessages?.Length > 0)
                problemDetails.Errors.Add("developerMessages",
                    developerMessages);

            if (userMessages?.Length > 0)
                problemDetails.Errors.Add("userMessages",
                    userMessages);

            return problemDetails;
        }
    }
}
