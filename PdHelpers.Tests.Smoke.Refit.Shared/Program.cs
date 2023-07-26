#define ASSEMBLY_LOGGING

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using XamarinFiles.FancyLogger;
using XamarinFiles.FancyLogger.Extensions;
using XamarinFiles.FancyLogger.Options;
using XamarinFiles.PdHelpers.Refit.Enums;
using XamarinFiles.PdHelpers.Refit.Models;
using static System.Net.HttpStatusCode;
using static XamarinFiles.PdHelpers.Refit.Enums.DetailsVariant;
using static XamarinFiles.PdHelpers.Refit.Enums.ErrorOrWarning;
using static XamarinFiles.PdHelpers.Refit.Extractors;

namespace XamarinFiles.PdHelpers.Tests.Smoke.Refit.Shared
{
    // TODO Simplify ErrorOrWarning reference after drop from FancyLogger
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
                    LogPrefixHelper.GetAssemblyNameTail(assembly,
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

                GenerateExceptionProblemReport();

                GenerateLoginProblemReport();

                //GenerateGeneralProblemReport();

                //GenerateJsonProblemReport();
            }
            catch (Exception exception)
            {
                FancyLogger?.LogException(exception);
            }
        }

        private static void GenerateExceptionProblemReport()
        {
            FancyLogger!.LogSection("Test Extracting Problem Report");

            var exception =
                new Exception("Outer Exception",
                    new Exception("Inner Exception"));
            var developerMessages =
                new[] { "Developer Message 1", "Developer Message 2" };
            var userMessages = new[] { "User Message 1", "User Message 2" };

            var errorExceptionProblemReport =
                ExtractProblemReport(exception,
                    Error,
                    developerMessages: developerMessages,
                    userMessages: userMessages);

            FancyLogger.LogProblemReport(errorExceptionProblemReport, Error);

            var warningExceptionProblemReport =
                ExtractProblemReport(exception,
                    Warning,
                    userMessages: userMessages);

            FancyLogger.LogProblemReport(warningExceptionProblemReport, Warning);

            // TODO Create or deserialize other exceptions to test:
            // - ApiException [request method, method, content(PD), statusCode, etc.]
            // - ValidationApiException [ApiException]
        }

        private static void GenerateLoginProblemReport()
        {
            FancyLogger!.LogSection("Test Bundling Problem Report");

            var fakeAppStateDetails =
                AppStateDetails.Create("Login Page", "Authentication");
            const string fakeUriStr = "/api/login";
            var fakeHttpRequestMessage =
                new HttpRequestMessage(HttpMethod.Post, fakeUriStr);
            const string fakeResourceName = "Login";

            // 400 - BadRequest - Error

            var badRequestProblem =
                ProblemReport.Create(BadRequest,
                    ValidationProblem,
                    Error,
                    fakeAppStateDetails,
                    fakeHttpRequestMessage,
                    fakeResourceName,
                    title: LoginFailedTitle,
                    detail: "Invalid fields: Username, Password",
                    instance: fakeUriStr,
                    developerMessages: new[]
                    {
                        "The Username field is required.",
                        "The Password field is required."
                    },
                    userMessages: LoginFailedUserMessages);

            FancyLogger.LogProblemReport(badRequestProblem, Warning);

            // 401 - Unauthorized - Warning

            var unauthorizedProblem =
                ProblemReport.Create(Unauthorized,
                    GenericProblem,
                    Warning,
                    fakeAppStateDetails,
                    fakeHttpRequestMessage,
                    fakeResourceName,
                    title: LoginFailedTitle,
                    detail : "Username and/or Password do not match",
                    instance: fakeUriStr,
                    userMessages: LoginFailedUserMessages);

            FancyLogger.LogProblemReport(unauthorizedProblem, Warning);

            // TODO Add other ProblemDetails tests from other repo
        }

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
    }
}
