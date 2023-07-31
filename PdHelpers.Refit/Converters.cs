using Refit;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.String;
using static System.Text.Json.JsonSerializer;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using static XamarinFiles.PdHelpers.Refit.Enums.ProblemLevel;

namespace XamarinFiles.PdHelpers.Refit
{
    // TODO Pass exception
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

        private const string DeveloperMessages = "developerMessages";

        private const string UserMessages = "userMessages";

        #endregion

        #region Methods

        // ProblemDetails object variant
        public static ProblemReport
            ConvertFromProblemDetails(ProblemDetails? problemDetails,
                ProblemVariant problemVariant,
                ProblemLevel problemLevel,
                string? assemblyName = null,
                string? componentName = null,
                string? operationName = null,
                ApiException? apiException = null,
                string? controllerName = null,
                string? resourceName = null)
        {
            ProblemReport problemReport;

            if (problemDetails is null)
            {
                problemReport =
                    CreateGenericProblemReport(Error, assemblyName,
                        componentName, operationName, apiException,
                        controllerName, resourceName);
            }
            else
            {
                var extensions = problemDetails.Extensions;
                var developerMessages =
                    GetExtensionsValue<string[]>(extensions, DeveloperMessages);
                var userMessages =
                    GetExtensionsValue<string[]>(extensions, UserMessages);

                problemReport =
                    ProblemReport.Create(problemDetails.Status, problemVariant,
                        problemLevel, assemblyName, componentName,
                        operationName, apiException, controllerName,
                        resourceName, problemDetails.Title,
                        problemDetails.Detail, problemDetails.Instance,
                        developerMessages, userMessages);
            }

            return problemReport;
        }

        // ProblemDetails string variant
        public static ProblemReport
            ConvertFromProblemDetails(string? problemDetailsStr,
                ProblemVariant problemVariant,
                ProblemLevel problemLevel,
                string? assemblyName = null,
                string? componentName = null,
                string? operationName = null,
                ApiException? apiException = null,
                string? controllerName = null,
                string? resourceName = null)
        {
            ProblemDetails? problemDetails = null;

            if (problemDetailsStr?.Trim() is not null)
            {
                try
                {
                    problemDetails =
                        Deserialize<ProblemDetails>(problemDetailsStr,
                            JsonSerializerReadOptions);
                }
// "Do not catch general exception types" doesn't apply.  Ignore all exceptions.
#pragma warning disable CA1031
                catch
#pragma warning restore CA1031
                {
                    problemDetails = null;
                }
            }

            var problemReport =
                ConvertFromProblemDetails(problemDetails, problemVariant,
                    problemLevel, assemblyName, componentName, operationName,
                    apiException, controllerName, resourceName);

            return problemReport;
        }

        private static T? GetExtensionsValue<T>(
            IDictionary<string, object> dictionary,
            string key)
        {
            if (!dictionary.TryGetValue(key, out var jsonObj))
                return default;

            try
            {
                // HACK Find more direct way if performance is an issue
                var jsonStr = Serialize(jsonObj);

                if (IsNullOrWhiteSpace(jsonStr))
                    return default;

                var obj = Deserialize<T>(jsonStr);

                return obj;
            }
// "Do not catch general exception types" doesn't apply.  Ignore all exceptions.
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
            {
                return default;
            }
        }

        #endregion
    }
}
