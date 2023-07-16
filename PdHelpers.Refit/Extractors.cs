using Refit;
using System;
using System.Net;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.Text.Json.JsonSerializer;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using static XamarinFiles.PdHelpers.Refit.Converters;
using static XamarinFiles.PdHelpers.Refit.Enums.DetailsVariant;
using RefitProblemDetails = Refit.ProblemDetails;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Extractors
    {
        public static ProblemReport
            ExtractProblemReport(Exception exception,
                string[] developerMessages = null,
                string[] userMessages = null)
        {
            ProblemReport problemReport;
            var exceptionMessages =
                ExtractExceptionMessages(exception);

            switch (exception)
            {
                case ValidationApiException { Content: { } } validationApiException:
                {
                    var problemDetails = validationApiException.Content;

                    problemReport =
                        ConvertFromProblemDetails(ValidationProblem,
                            problemDetails, developerMessages, userMessages,
                            exceptionMessages);

                    break;
                }
                case ApiException apiException when
                    !string.IsNullOrWhiteSpace(apiException.Content):
                    try
                    {
                        var problemDetails =
                            Deserialize<RefitProblemDetails>(apiException.Content);

                        problemReport =
                            ConvertFromProblemDetails(
                                GenericProblem, problemDetails,
                                developerMessages, userMessages, exceptionMessages);
                    }
                    catch
                    {
                        problemReport =
                            CreateGenericProblemReport(developerMessages,
                                userMessages, exceptionMessages);
                    }

                    break;
                default:
                    problemReport =
                        CreateGenericProblemReport(developerMessages,
                            userMessages, exceptionMessages);

                    break;
            }

            return problemReport;
        }

        private static ProblemReport
            CreateGenericProblemReport(string[] developerMessages,
                string[] userMessages, ExceptionMessages exceptionMessages)
        {
            var problemReport =
                BundleProblemReport(GenericProblem,
                    HttpStatusCode.InternalServerError,
                    developerMessages: developerMessages,
                    userMessages: userMessages,
                    exceptionMessages: exceptionMessages);

            return problemReport;
        }

        // TODO Extend for AggregateExceptions and others
        private static ExceptionMessages ExtractExceptionMessages(Exception exception)
        {
            if (exception == null)
                return null;

            var exceptionMessages = new ExceptionMessages
            {
                ExceptionMessage = exception.Message
            };

            if (!string.IsNullOrWhiteSpace(exception.InnerException?.Message))
            {
                exceptionMessages.InnerExceptionMessage =
                    exception.InnerException.Message;
            }

            // TODO Return other messages

            return exceptionMessages;
        }
    }
}
