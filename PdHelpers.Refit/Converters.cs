using System;
using Refit;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.String;
using static System.Text.Json.JsonSerializer;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using static XamarinFiles.PdHelpers.Refit.Enums.ErrorOrWarning;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Converters
    {
        #region Fields

        private static readonly JsonSerializerOptions
            JsonSerializerReadOptions = new()
            {
                AllowTrailingCommas = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true
            };

        #endregion

        #region Methods

        public static ProblemReport
            ConvertFromProblemDetails(ProblemDetails? problemDetails,
                DetailsVariant detailsVariant,
                ErrorOrWarning errorOrWarning,
                AppStateDetails? appStateDetails = null,
                HttpRequestMessage? requestMessage = null,
                string? resourceName = null,
                string[]? developerMessages = null,
                string[]? userMessages = null,
                ExceptionMessages? exceptionMessages = null)
        {
            ProblemReport problemReport;

            if (problemDetails is null)
            {
                problemReport =
                    CreateGenericProblemReport(appStateDetails,
                        developerMessages, userMessages, exceptionMessages);
            }
            else
            {
                problemReport =
                    ProblemReport.Create(
                        problemDetails.Status,
                        detailsVariant,
                        errorOrWarning,
                        appStateDetails,
                        requestMessage,
                        resourceName,
                        problemDetails.Title,
                        problemDetails.Detail,
                        problemDetails.Instance,
                        developerMessages,
                        userMessages,
                        exceptionMessages);
            }

            return problemReport;
        }

        public static ProblemReport
            ConvertFromProblemDetails(string? problemDetailsStr,
                DetailsVariant detailsVariant,
                ErrorOrWarning errorOrWarning,
                AppStateDetails? appStateDetails = null,
                HttpRequestMessage? requestMessage = null,
                string? resourceName = null,
                string[]? developerMessages = null,
                string[]? userMessages = null,
                ExceptionMessages? exceptionMessages = null)
        {
            ProblemReport problemReport;

            // Redundant Is-Null check to silence .NET Std 2.0 compiler warning
            if (problemDetailsStr is null
                || IsNullOrWhiteSpace(problemDetailsStr))
            {
                problemReport =
                    CreateGenericProblemReport(appStateDetails,
                        developerMessages, userMessages, exceptionMessages);
            }
            else
            {
                try
                {
                    var problemDetails =
                        Deserialize<ProblemDetails>(problemDetailsStr,
                            JsonSerializerReadOptions);

                    problemReport =
                        ProblemReport.Create(
                            problemDetails!.Status,
                            detailsVariant,
                            errorOrWarning,
                            appStateDetails,
                            requestMessage,
                            resourceName,
                            problemDetails.Title,
                            problemDetails.Detail,
                            problemDetails.Instance,
                            developerMessages,
                            userMessages,
                            exceptionMessages);
                }
                catch (Exception exception)
                {
                    // TODO Add context about unidentified input text <> PD/VPD
                    problemReport =
                        Extractors.ExtractProblemReport(exception,
                            Error, appStateDetails,
                            "ProblemDetails Converter",
                            developerMessages, userMessages);
                }
            }

            return problemReport;
        }

        #endregion
    }
}
