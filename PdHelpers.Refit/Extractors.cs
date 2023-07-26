using Refit;
using System;
using XamarinFiles.PdHelpers.Refit.Enums;
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
                ErrorOrWarning errorOrWarning,
                string? sourceOperation = null,
                string? resourceName = null,
                string[]? developerMessages = null,
                string[]? userMessages = null)
        {
            ProblemReport problemReport;

            // TODO Pull exception info directly or pass exception to converter
            var sourceDetails = SourceDetails.Create(exception, sourceOperation);
            var exceptionMessages = ExceptionMessages.Create(exception);

            // TODO Pull from ApiException: full Uri?
            switch (exception)
            {
                case ValidationApiException { Content: { } } validationApiException:
                {
                    var problemDetails = validationApiException.Content;

                    problemReport =
                        ConvertFromProblemDetails(problemDetails,
                            ValidationProblem,
                            errorOrWarning,
                            sourceName: sourceDetails?.Name,
                            sourceLocation: sourceDetails?.Location,
                            sourceOperation,
                            validationApiException.RequestMessage,
                            resourceName,
                            developerMessages,
                            userMessages,
                            exceptionMessages);

                    break;
                }
                // TODO Add check for ApiException.Content is not PD/VPD
                case ApiException apiException when
                    !string.IsNullOrWhiteSpace(apiException.Content):

                    var problemDetailsStr = apiException.Content;

                    problemReport =
                        ConvertFromProblemDetails(problemDetailsStr,
                            GenericProblem,
                            errorOrWarning,
                            sourceName: sourceDetails?.Name,
                            sourceLocation: sourceDetails?.Location,
                            sourceOperation,
                            apiException.RequestMessage,
                            resourceName,
                            developerMessages,
                            userMessages,
                            exceptionMessages);

                    break;
                default:
                    problemReport =
                        CreateGenericProblemReport(
                            sourceName: sourceDetails?.Name,
                            sourceLocation: sourceDetails?.Location,
                            sourceOperation,
                            developerMessages,
                            userMessages,
                            exceptionMessages);

                    break;
            }

            return problemReport;
        }
    }

    #endregion
}
