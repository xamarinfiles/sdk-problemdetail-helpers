using Refit;
using System;
using System.Collections.Generic;
using static System.Net.HttpStatusCode;
using static System.Text.Json.JsonSerializer;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using RefitProblemDetails = Refit.ProblemDetails;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Extractors
    {
        public static RefitProblemDetails
            ExtractRefitProblemDetails(
            Exception exception,
            string[] developerMessages = null,
            string[] userMessages = null)
        {
            ProblemDetails problemDetails;

            if (exception is ApiException apiException &&
                !string.IsNullOrWhiteSpace(apiException.Content))
            {
                try
                {
                    problemDetails =
                        Deserialize<ProblemDetails>(apiException.Content);

                    return problemDetails;
                }
                catch
                {
                    // ignored
                }
            }

            var exceptionMessages = ExtractExceptionMessages(exception);

            problemDetails =
                BundleRefitProblemDetails(InternalServerError,
                    exceptionMessages: exceptionMessages,
                    developerMessages: developerMessages,
                    userMessages: userMessages);

            return problemDetails;
        }

        // TODO Make Debug only?
        // TODO Extend for AggregateExceptions and others
        private static string[] ExtractExceptionMessages(Exception exception)
        {
            var exceptionMessages = new List<string> { exception.Message };

            if (exception.InnerException is { } innerException)
            {
                exceptionMessages.Add(innerException.Message);
            }

            return exceptionMessages.ToArray();
        }
    }
}
