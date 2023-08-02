using Refit;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.String;
using static System.Text.Json.JsonSerializer;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using static XamarinFiles.PdHelpers.Refit.Enums.ProblemLevel;

// Ignore all JSON serialization/deserialization exceptions here as bad data
#pragma warning disable CA1031

namespace XamarinFiles.PdHelpers.Refit
{
    // TODO Make converter smart enough to adjust to ProblemVariant of PD vs VPD
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

        private const string DevMessagesKey = "developerMessages";

        private const string UserMessagesKey = "userMessages";

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
                Exception? exception = null,
                string? controllerName = null,
                string? resourceName = null)
        {
            ProblemReport problemReport;

            if (problemDetails is null)
            {
                problemReport =
                    CreateGenericProblemReport(Error,
                        assemblyName,
                        componentName,
                        operationName,
                        exception,
                        controllerName,
                        resourceName);
            }
            else
            {
                var (developerMessages, userMessages) =
                    GetMessages(problemDetails, problemVariant);

                problemReport =
                    ProblemReport.Create(problemDetails.Status,
                        problemVariant,
                        problemLevel,
                        assemblyName,
                        componentName,
                        operationName,
                        exception,
                        controllerName,
                        resourceName,
                        problemDetails.Title,
                        problemDetails.Detail,
                        problemDetails.Instance,
                        developerMessages,
                        userMessages);
            }

            return problemReport;
        }

        // TODO Pull other context info when ApiException.Content string is not PD
        // ProblemDetails string variant
        public static ProblemReport
            ConvertFromProblemDetails(string? problemDetailsStr,
                ProblemVariant problemVariant,
                ProblemLevel problemLevel,
                string? assemblyName = null,
                string? componentName = null,
                string? operationName = null,
                Exception? exception = null,
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
                catch
                {
                    problemDetails = null;
                }
            }

            var problemReport =
                ConvertFromProblemDetails(problemDetails,
                    problemVariant,
                    problemLevel,
                    assemblyName,
                    componentName,
                    operationName,
                    exception,
                    controllerName,
                    resourceName);

            return problemReport;
        }

        private static (string[]?, string[]?)
            GetMessages(ProblemDetails problemDetails,
                ProblemVariant problemVariant)
        {
            string[]? developerMessages;
            string[]? userMessages;

            switch (problemVariant)
            {
                case ProblemVariant.GenericProblem:
                    var extensions = problemDetails.Extensions;

                    developerMessages =
                        GetExtensionsValue<string[]>(extensions, DevMessagesKey);
                    userMessages =
                        GetExtensionsValue<string[]>(extensions, UserMessagesKey);

                    break;
                case ProblemVariant.ValidationProblem:
                    var errors = problemDetails.Errors;

                    developerMessages =
                        GetErrorsValue(errors, DevMessagesKey);
                    userMessages =
                        GetErrorsValue(errors, UserMessagesKey);

                    break;
                default:
                    developerMessages = null;
                    userMessages = null;

                    break;
            }

            return (developerMessages, userMessages);

        }

        private static string[]?
            GetErrorsValue(IDictionary<string, string[]> errors,
                string key)
        {
            if (!errors.TryGetValue(key, out var jsonObj))
                return default;

            try
            {
                // HACK Find more direct way if performance is an issue
                var jsonStr = Serialize(jsonObj);

                if (IsNullOrWhiteSpace(jsonStr))
                    return default;

                var strArray = Deserialize<string[]>(jsonStr);

                return strArray;
            }
            catch
            {
                return default;
            }

        }


        private static T?
            GetExtensionsValue<T>(IDictionary<string, object> extensions,
                string key)
        {
            if (!extensions.TryGetValue(key, out var jsonObj))
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
            catch
            {
                return default;
            }
        }

        #endregion
    }
}
