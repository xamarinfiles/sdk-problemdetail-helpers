#define ASSEMBLY_LOGGING

using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;
using XamarinFiles.FancyLogger;
using XamarinFiles.FancyLogger.Extensions;
using XamarinFiles.FancyLogger.Options;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.Net.HttpStatusCode;
using static System.Text.Json.JsonSerializer;
using static XamarinFiles.PdHelpers.Refit.Converters;
using static XamarinFiles.PdHelpers.Refit.Enums.ProblemLevel;
using static XamarinFiles.PdHelpers.Refit.Enums.ProblemVariant;

// TODO Find better way to simplify overlap with FancyLogger (shared models NuGet?)

// To accommodate the Local and NuGet variant of the smoke tester with minimal
// interruptions, the following concessions will be made:
//
// PdHelpers.Tests.Smoke.Refit.Local:
//
// 1.   The Local project will use a ProjectReference to PdHelpers.Refit in this
//      repo to facilitate testing current changes
// 2.   The Local project will use a Reference to a local copy of the XamFiles
//      FancyLogger repo to allow testing the overlap of both repos together
// 3.   The FancyLogger library itself has a reference to PdHelpers.Refit, so
//      pushing a PdHelpers.Refit update to NuGet may be required before changes
//      to FancyLogger can be implemented
// 4.   The logic of the Local test Program will be updated for the current
//      changes to PdHelpers.Refit and FancyLogger and copied to the NuGet
//      test Program after any updated release of one or both NuGets
//
// PdHelpers.Tests.Smoke.Refit.NuGet:
//
// 1.   The NuGet project will use NuGet references to PdHelpers.Refit and
//      FancyLogger to test the released version of both packages together
// 2.   The logic of the NuGet test Program will be appropriate for the released
//      versions of PdHelpers.Refit and FancyLogger and copied from the Local
//      test Program after any updated release of one or both NuGets
//
// After a PdHelpers.Refit release, the only difference between the Local and
// NuGet copies of the test Program should be the different namespaces.

namespace XamarinFiles.PdHelpers.Tests.Smoke.Refit.Local
{
    internal static class Program
    {
        #region Fields

        // TODO Set to same default as FancyLoggerOptions.AllLines.PrefixString
        private const string DefaultLogPrefix = "LOG";

        private const string LoginFailedTitle = "Invalid Credentials";

        private static readonly string[] LoginFailedUserMessages =
        {
            "Please check your Username and Password and try again"
        };

        private const string RootAssemblyNamespace = "XamarinFiles.PdHelpers.";

        // TODO TEMP
        private static readonly JsonSerializerOptions
            DefaultWriteJsonOptions = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };

        #endregion

        #region Services

        private static IFancyLogger? FancyLogger { get; }

#if ASSEMBLY_LOGGING
        private static AssemblyLogger? AssemblyLogger { get; }
#endif

        #endregion

        #region Constructor

        static Program()
        {
            try
            {
                var assembly = typeof(Program).Assembly;

                // ReSharper disable once UseObjectOrCollectionInitializer
                var options = new FancyLoggerOptions();
                options.AllLines.PrefixString =
                    PrefixHelper.GetAssemblyNameTail(assembly,
                        RootAssemblyNamespace, DefaultLogPrefix);

                FancyLogger = new FancyLogger.FancyLogger(loggerOptions: options);
#if ASSEMBLY_LOGGING
                AssemblyLogger = new AssemblyLogger(FancyLogger);
#endif
            }
            catch (Exception exception)
            {
                if (FancyLogger != null)
                {
                    FancyLogger.LogException(exception);
                }
                else
                {
                    Debug.WriteLine("ERROR: Problem setting up logging services");
                    Debug.WriteLine(exception);
                }
            }
        }

        #endregion

        public static void Main()
        {
            try
            {
                if (FancyLogger is null)
                {
                    return;
                }

#if ASSEMBLY_LOGGING
                AssemblyLogger?.LogAssemblies(showCultureInfo: true);
#endif

                FancyLogger.LogSection("Run PdHelpers Refit Tests");

                TestParseProblemDetails();

                //GenerateExceptionProblemReport();

                //GenerateGeneralProblemReport();

                //GenerateJsonProblemReport();

                TestGenerateLoginProblemReport();
            }
            catch (Exception exception)
            {
                FancyLogger?.LogException(exception);
            }
        }

        private static void TestParseProblemDetails()
        {
            FancyLogger!.LogSection(
                "Test Extracting Problem Reports");

            FancyLogger.LogSubsection(
                "Test Extracting Problem Report From Validation Problem Details");

            var jsonValidationProblemDetails = "{\r\n    \"errors\": {\r\n        \"developerMessages\": [\r\n            \"Unexpected character encountered while parsing value: ,. Path 'userName', line 2, position 16.\"\r\n        ],\r\n        \"userMessages\": [\r\n            \"Please send a properly-formatted JSON object and try again\"\r\n        ]\r\n    },\r\n    \"type\": \"https://tools.ietf.org/html/rfc7231#section-6.5.1\",\r\n    \"title\": \"Bad Request\",\r\n    \"status\": 400,\r\n    \"detail\": \"Invalid fields: userName\",\r\n    \"instance\": \"/api/authentication/login\"\r\n}";

            var validationProblemDetails =
                Deserialize<ProblemDetails>(jsonValidationProblemDetails);
            FancyLogger.LogTrace(
                Serialize(validationProblemDetails, DefaultWriteJsonOptions),
                newLineAfter: true);

            var jsonValidationProblemDetailsReport =
                ConvertFromProblemDetails(jsonValidationProblemDetails,
                    ValidationProblem,
                    Error);

            FancyLogger.LogProblemReport(jsonValidationProblemDetailsReport);

            FancyLogger.LogSubsection(
                "Test Extracting Problem Report From Problem Details");

            var jsonProblemDetails = "{\r\n  \"Errors\": {},\r\n  \"Type\": \"https://tools.ietf.org/html/rfc7235#section-3.1\",\r\n  \"Title\": \"Invalid Credentials\",\r\n  \"Status\": 401,\r\n  \"Detail\": \"Username and/or Password do not match\",\r\n  \"Instance\": \"/api/authentication/login\",\r\n  \"userMessages\": [\r\n    \"Please check your Username and Password and try again\"\r\n  ]\r\n}";

            var problemDetails = Deserialize<ProblemDetails>(jsonProblemDetails);
            FancyLogger.LogTrace(
                Serialize(problemDetails, DefaultWriteJsonOptions),
                newLineAfter: true);

            var jsonProblemDetailsReport =
                ConvertFromProblemDetails(jsonProblemDetails,
                    GenericProblem,
                    Error);

            FancyLogger.LogProblemReport(jsonProblemDetailsReport);

        }

        // TODO
        //private static void GenerateExceptionProblemReport()
        //{
        //    // TODO
        //}

        // TODO
        //private static void GenerateGeneralProblemReport()
        //{
        //    // TODO
        //}

        // TODO
        //private static void GenerateJsonProblemReport()
        //{
        //    // TODO
        //}

        private static void TestGenerateLoginProblemReport()
        {
            FancyLogger!.LogSection("Test Creating Problem Report");

            const string fakeAssemblyName = "PdHelpers Smoke Tester";
            const string fakeComponentName = "Login Page";
            const string fakeOperationName = "Authentication";
            const string fakeUriStr = "/api/authentication/login";
            const string fakeControllerName = "Authentication";
            const string fakeResourceName = "Login";

            // 400 - BadRequest - Error

            var badRequestProblem =
                ProblemReport.Create(BadRequest,
                    ValidationProblem,
                    Error,
                    fakeAssemblyName,
                    fakeComponentName,
                    fakeOperationName,
                    controllerName: fakeControllerName,
                    resourceName: fakeResourceName,
                    title: LoginFailedTitle,
                    detail: "Invalid fields: Username, Password",
                    instance: fakeUriStr,
                    developerMessages: new[]
                    {
                        "The Username field is required.",
                        "The Password field is required."
                    },
                    userMessages: LoginFailedUserMessages);

            FancyLogger.LogProblemReport(badRequestProblem);

            // 401 - Unauthorized - Warning

            var unauthorizedProblem =
                ProblemReport.Create(Unauthorized,
                    GenericProblem,
                    Warning,
                    fakeAssemblyName,
                    fakeComponentName,
                    fakeOperationName,
                    controllerName: fakeControllerName,
                    resourceName: fakeResourceName,
                    title: LoginFailedTitle,
                    detail : "Username and/or Password do not match",
                    instance: fakeUriStr,
                    userMessages: LoginFailedUserMessages);

            FancyLogger.LogProblemReport(unauthorizedProblem);

            // TODO Add other ProblemDetails tests from other repo
        }
    }
}
