using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.Globalization.CultureInfo;
using static System.String;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using static XamarinFiles.PdHelpers.Refit.Converters;
using static XamarinFiles.PdHelpers.Refit.Enums.ProblemLevel;
using static XamarinFiles.PdHelpers.Refit.Enums.ProblemVariant;

namespace XamarinFiles.PdHelpers.Refit
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Extractors
    {
        #region Exception to ProblemReport

        public static ProblemReport?
            ExtractProblemReport(Exception? exception,
                ProblemLevel problemLevel,
                string? assemblyName = null,
                string? componentName = null,
                string? operationName = null,
                string? controllerName = null,
                string? resourceName = null,
                string[]? developerMessages = null,
                string[]? userMessages = null)
        {
            if (exception == null)
                return null;

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
                    !IsNullOrWhiteSpace(apiException.Content):

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

        #endregion

        #region ProblemReport to Dictionary for VS App Center

        public static (string?, Dictionary<string, string>?)
            ExtractEventContext(ProblemReport? problemReport)
        {
            if (problemReport == null)
            {
                return (null, null);
            }

            var eventContext = new Dictionary<string, string>
            {
                { "Report.ProblemVariant", problemReport.ProblemVariant },
                { "Report.ProblemLevel", problemReport.ProblemLevel },
            };

            var sourceDetails = problemReport.SourceDetails;
            AddUsefulString("Source.Assembly", sourceDetails?.AssemblyName);
            AddUsefulString("Source.Component", sourceDetails?.ComponentName);
            AddUsefulString("Source.Operation", sourceDetails?.OperationName);

            var exceptionDetails = problemReport.ExceptionDetails;
            AddUsefulString("Exception.Assembly", exceptionDetails?.Assembly);
            AddUsefulString("Exception.Method", exceptionDetails?.Method);
            var exceptionMessages = exceptionDetails?.Messages;
            AddUsefulString("Exception.Message.OuterException",
                exceptionMessages?.OuterException);
            AddUsefulString("Exception.Message.InnerException",
                exceptionMessages?.InnerException);
            // TODO Add local function to include other messages when save from exceptions

            var requestDetails = problemReport.RequestDetails;
            var requestMethod = requestDetails?.Method?.ToUpper(InvariantCulture);
            AddUsefulString("Request.Method", requestMethod);
            var requestController = requestDetails?.Controller;
            AddUsefulString("Request.Controller", requestController);
            var requestResource = requestDetails?.Resource;
            AddUsefulString("Request.Resource", requestResource);

            var responseDetails = problemReport.ResponseDetails;
            var statusCodeStr = responseDetails?.StatusCodeInt.ToString(InvariantCulture);
            AddUsefulString("Response.StatusCode", statusCodeStr);
            var statusCodeTitle = responseDetails?.StatusTitle;
            AddUsefulString("Response.StatusTitle", statusCodeTitle);
            AddUsefulString("Response.InstanceUri", responseDetails?.InstanceUri);
            AddUsefulString("Response.ProblemSummary", responseDetails?.ProblemSummary);
            AddUsefulString("Response.ProblemExplanation",
                responseDetails?.ProblemExplanation);

            // TODO Pull Extensions and OtherErrors when add

            var eventSummary = FormatEventName();

            return (eventSummary, eventContext);

            void AddUsefulString(string key, string? value)
            {
                if (IsNullOrWhiteSpace(value))
                    return;

                eventContext.Add(key, value!);
            }

            string FormatEventName()
            {
                char[] trimChars = { ' ', '-' };
                var tempStr = Empty;

                var hasRequestMethod = !IsNullOrWhiteSpace(requestMethod);
                var hasRequestController = !IsNullOrWhiteSpace(requestController);
                var hasRequestResource = !IsNullOrWhiteSpace(requestResource);
                var hasStatusCodeStr = !IsNullOrWhiteSpace(statusCodeStr);
                var hasStatusCodeTitle = !IsNullOrWhiteSpace(statusCodeTitle);

                tempStr  += hasRequestMethod ? requestMethod + " " : Empty;
                tempStr  += hasRequestController ? requestController + " " : Empty;
                tempStr  += hasRequestResource ? requestResource + " " : Empty;

                if (tempStr .Length > 0)
                {
                    tempStr  += "- ";
                }

                tempStr  += hasStatusCodeStr ? statusCodeStr + " - " : Empty;

                tempStr  += hasStatusCodeTitle ? statusCodeTitle + " - " : Empty;

                var finalStr  = tempStr.TrimEnd(trimChars);

                return finalStr;
            }
        }

        #endregion
    }
}