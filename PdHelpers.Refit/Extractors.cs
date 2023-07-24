using Refit;
using System;
using XamarinFiles.PdHelpers.Refit.Models;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using static XamarinFiles.PdHelpers.Refit.Converters;
using static XamarinFiles.PdHelpers.Refit.Enums.DetailsVariant;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Extractors
    {
        #region Methods

        public static ProblemReport
            ExtractProblemReport(Exception exception,
                string[]? developerMessages = null,
                string[]? userMessages = null)
        {
            ProblemReport problemReport;
            var exceptionMessages =
                ExtractExceptionMessages(exception);

            // TODO Pull from ApiException: full Uri?
            switch (exception)
            {
                case ValidationApiException { Content: { } } validationApiException:
                {
                    var problemDetails = validationApiException.Content;
                    var validationHttpMethodName =
                        validationApiException.HttpMethod.Method;

                    problemReport =
                        ConvertFromProblemDetails(ValidationProblem,
                            problemDetails, validationHttpMethodName,
                            developerMessages, userMessages, exceptionMessages);

                    break;
                }
                case ApiException apiException when
                    !string.IsNullOrWhiteSpace(apiException.Content):

                    var problemDetailsStr = apiException.Content;
                    var genericHttpMethodName =
                        apiException.HttpMethod.Method;

                    problemReport =
                        ConvertFromProblemDetails(GenericProblem,
                            problemDetailsStr, genericHttpMethodName,
                            developerMessages, userMessages, exceptionMessages);

                    break;
                default:
                    problemReport =
                        CreateGenericProblemReport(developerMessages,
                            userMessages, exceptionMessages);

                    break;
            }

            return problemReport;
        }

        // TODO Extend for AggregateExceptions and others
        private static ExceptionMessages?
            ExtractExceptionMessages(Exception? exception)
        {
            if (exception is null)
                return null;

            var exceptionMessages = new ExceptionMessages
            {
                ExceptionMessage = exception.Message,
                InnerExceptionMessage = exception.InnerException?.Message
            };

            // TODO Return other messages

            return exceptionMessages;
        }
    }

    #endregion
}
