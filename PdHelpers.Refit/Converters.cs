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
                string? sourceName = null,
                string? sourceLocation = null,
                string? sourceOperation = null,
                HttpRequestMessage? requestMessage = null,
                string? resourceName = null,
                string[]? developerMessages = null,
                string[]? userMessages = null,
                ExceptionMessages? exceptionMessages = null)
        {
            ProblemReport problemReport;

            var sourceDetails =
                SourceDetails.Create(sourceName, sourceLocation, sourceOperation);

            if (problemDetails is null)
            {
                problemReport =
                    CreateGenericProblemReport(
                        sourceName,
                        sourceLocation,
                        sourceOperation,
                        developerMessages,
                        userMessages,
                        exceptionMessages);
            }
            else
            {
                problemReport =
                    ProblemReport.Create(
                        problemDetails.Status,
                        detailsVariant,
                        errorOrWarning,
                        sourceDetails,
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
                string? sourceName = null,
                string? sourceLocation = null,
                string? sourceOperation = null,
                HttpRequestMessage? requestMessage = null,
                string? resourceName = null,
                string[]? developerMessages = null,
                string[]? userMessages = null,
                ExceptionMessages? exceptionMessages = null)
        {
            ProblemReport problemReport;

            var sourceDetails =
                SourceDetails.Create(sourceName, sourceLocation, sourceOperation);

            // Redundant Is-Null check to silence .NET Std 2.0 compiler warning
            if (problemDetailsStr is null
                || IsNullOrWhiteSpace(problemDetailsStr))
            {
                problemReport =
                    CreateGenericProblemReport(
                        sourceName,
                        sourceLocation,
                        sourceOperation,
                        developerMessages,
                        userMessages,
                        exceptionMessages);
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
                            sourceDetails,
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
                            Error,
                            sourceOperation: "Extract Exception",
                            developerMessages: developerMessages,
                            userMessages: userMessages);
                }
            }

            return problemReport;
        }

        #endregion
    }
}
