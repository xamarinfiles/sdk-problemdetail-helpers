using System.Collections.Generic;
using System.Net;
using static XamarinFiles.PdHelpers.Shared.StatusCodeDetails;
using RefitProblemDetails = Refit.ProblemDetails;

namespace XamarinFiles.PdHelpers.Refit
{
    //[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    //[SuppressMessage("ReSharper", "UnusedType.Global")]
    public static class Bundlers
    {
        public static RefitProblemDetails
            BundleRefitProblemDetails(
            HttpStatusCode statusCode,
            string title = null,
            string detail = null,
            string instance = null,
            string[] developerMessages = null,
            string[] userMessages = null,
            string[] exceptionMessages = null
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

            if (developerMessages != null)
                problemDetails.Errors.Add("developerMessages",
                    developerMessages);

            if (userMessages != null)
                problemDetails.Errors.Add("userMessages",
                    userMessages);

            if (exceptionMessages != null)
                problemDetails.Errors.Add("exceptionMessages",
                    exceptionMessages);

            return problemDetails;
        }
    }
}
