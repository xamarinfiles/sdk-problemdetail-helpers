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
                AppStateDetails? appStateDetails = null,
                string? resourceName = null,
                string[]? developerMessages = null,
                string[]? userMessages = null)
        {
            ProblemReport problemReport;
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
                            appStateDetails,
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
                            appStateDetails,
                            apiException.RequestMessage,
                            resourceName,
                            developerMessages,
                            userMessages,
                            exceptionMessages);

                    break;
                default:
                    problemReport =
                        CreateGenericProblemReport(appStateDetails,
                            developerMessages, userMessages, exceptionMessages);

                    break;
            }

            return problemReport;
        }
    }

    #endregion
}
