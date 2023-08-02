using Refit;
using System;
using System.Diagnostics.CodeAnalysis;
using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using static XamarinFiles.PdHelpers.Refit.Converters;
using static XamarinFiles.PdHelpers.Refit.Enums.ProblemVariant;
using static XamarinFiles.PdHelpers.Refit.Enums.ProblemLevel;

namespace XamarinFiles.PdHelpers.Refit
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Extractors
    {
        #region Methods

        public static ProblemReport
            ExtractProblemReport(Exception exception,
                ProblemLevel problemLevel,
                string? assemblyName = null,
                string? componentName = null,
                string? operationName = null,
                string? controllerName = null,
                string? resourceName = null,
                string[]? developerMessages = null,
                string[]? userMessages = null)
        {
            ProblemReport problemReport;

            // TODO Pull from ValidationApiException/ApiException: full Uri?
            switch (exception)
            {
                case ValidationApiException { Content: not null } validationApiException:
                {
                    var problemDetails = validationApiException.Content;

                    problemReport =
                        ConvertFromProblemDetails(problemDetails,
                            ValidationProblem,
                            problemLevel,
                            assemblyName,
                            componentName,
                            operationName,
                            validationApiException,
                            controllerName,
                            resourceName);

                    break;
                }
                // TODO Add check for ApiException.Content is not PD/VPD
                case ApiException apiException when
                    !string.IsNullOrWhiteSpace(apiException.Content):

                    var problemDetailsStr = apiException.Content;

                    problemReport =
                        ConvertFromProblemDetails(problemDetailsStr,
                            GenericProblem,
                            problemLevel,
                            assemblyName,
                            componentName,
                            operationName,
                            apiException,
                            controllerName,
                            resourceName);

                    break;
                default:
                    problemReport =
                        CreateGenericProblemReport(Error,
                            assemblyName,
                            componentName,
                            operationName,
                            exception,
                            controllerName,
                            resourceName,
                            developerMessages,
                            userMessages);

                    break;
            }

            return problemReport;
        }
    }

    #endregion
}
