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
            string type = null;

            if (HttpStatusDetails.ContainsKey(statusCodeInt))
            {
                title ??= HttpStatusDetails[statusCodeInt].Title;
                type = HttpStatusDetails[statusCodeInt].Type;
            }
            else
            {
                title = statusCode.ToString();
            }

            var problemDetails = new RefitProblemDetails
            {
                Status = statusCodeInt,
                Title = title,
                Detail = detail,
                Type = type,
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
