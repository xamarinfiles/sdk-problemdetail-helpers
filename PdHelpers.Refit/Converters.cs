using System.Text.Json;
using System.Text.Json.Serialization;
using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.String;
using static System.Text.Json.JsonSerializer;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using RefitProblemDetails = Refit.ProblemDetails;

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
            ConvertFromProblemDetails(DetailsVariant detailsVariant,
                RefitProblemDetails? problemDetails, string httpMethod,
                string[]? developerMessages, string[]? userMessages,
                ExceptionMessages? exceptionMessages)
        {
            ProblemReport problemReport;

            if (problemDetails is null)
            {
                problemReport = CreateGenericProblemReport(developerMessages,
                    userMessages, exceptionMessages);
            }
            else
            {
                problemReport = BundleProblemReport(detailsVariant,
                    problemDetails.Status,
                    problemDetails.Title,
                    problemDetails.Detail,
                    problemDetails.Instance,
                    problemDetails.Type,
                    httpMethod,
                    developerMessages,
                    userMessages,
                    exceptionMessages);
            }

            return problemReport;
        }

        public static ProblemReport
            ConvertFromProblemDetails(DetailsVariant detailsVariant,
                string? problemDetailsStr, string httpMethod,
                string[]? developerMessages, string[]? userMessages,
                ExceptionMessages? exceptionMessages)
        {
            ProblemReport problemReport;

            // Redundant Is-Null check to silence .NET Std 2.0 compiler warning
            if (problemDetailsStr is null || IsNullOrWhiteSpace(problemDetailsStr))
            {
                problemReport = CreateGenericProblemReport(developerMessages,
                    userMessages, exceptionMessages);
            }
            else
            {
                try
                {
                    var problemDetails =
                        Deserialize<RefitProblemDetails>(problemDetailsStr,
                            JsonSerializerReadOptions);

                    problemReport = BundleProblemReport(detailsVariant,
                        problemDetails!.Status,
                        problemDetails.Title,
                        problemDetails.Detail,
                        problemDetails.Instance,
                        problemDetails.Type,
                        httpMethod,
                        developerMessages,
                        userMessages,
                        exceptionMessages);
                }
                catch
                {
                    problemReport = CreateGenericProblemReport(developerMessages,
                        userMessages, exceptionMessages);
                }
            }

            return problemReport;
        }

        #endregion
    }
}
