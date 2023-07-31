﻿using Refit;
using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.Net.HttpStatusCode;
using static XamarinFiles.PdHelpers.Refit.Enums.ProblemVariant;

namespace XamarinFiles.PdHelpers.Refit
{
    public static class Bundlers
    {
        // TODO Add more context
        // TODO Overwrite developer/user messages or add additional?
        internal static ProblemReport
            CreateGenericProblemReport(
                ProblemLevel problemLevel,
                string? assemblyName = null,
                string? componentName = null,
                string? operationName = null,
                ApiException? apiException = null,
                string? controllerName = null,
                string? resourceName = null)
        {
            var problemReport =
                ProblemReport.Create(InternalServerError,
                    GenericProblem,
                    problemLevel,
                    assemblyName,
                    componentName,
                    operationName,
                    apiException,
                    controllerName,
                    resourceName);

            return problemReport;
        }
    }
}
