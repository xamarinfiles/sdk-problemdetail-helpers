using System;
using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.Net.HttpStatusCode;
using static XamarinFiles.PdHelpers.Refit.Enums.ProblemVariant;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Bundlers
    {
        // TODO Overwrite developer/user messages or add additional?
        internal static ProblemReport
            CreateGenericProblemReport(
                ProblemLevel problemLevel,
                string? assemblyName = null,
                string? componentName = null,
                string? operationName = null,
                Exception? exception = null,
                string? controllerName = null,
                string? resourceName = null,
                string[]? developerMessages = null,
                string[]? userMessages = null)
        {
            var problemReport =
                ProblemReport.Create(InternalServerError,
                    GenericProblem,
                    problemLevel,
                    assemblyName,
                    componentName,
                    operationName,
                    exception,
                    controllerName,
                    resourceName,
                    developerMessages: developerMessages,
                    userMessages: userMessages);

            return problemReport;
        }
    }
}
