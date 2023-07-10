#define ASSEMBLY_LOGGING

using System;
using System.Diagnostics;
using XamarinFiles.FancyLogger;
using XamarinFiles.FancyLogger.Extensions;
using XamarinFiles.FancyLogger.Options;
using static XamarinFiles.PdHelpers.Refit.Bundlers;
using static System.Net.HttpStatusCode;

namespace XamarinFiles.PdHelpers.Tests.Smoke.Shared
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
                if (FancyLogger is not null)
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


                GenerateGeneralProblemDetails();

                GenerateJsonProblemDetails();

                GenerateLoginProblemDetails();
            }
            catch (Exception exception)
            {
                FancyLogger?.LogException(exception);
            }
        }

        private static void GenerateGeneralProblemDetails()
        {
            // TODO
        }

        private static void GenerateJsonProblemDetails()
        {
            // TODO
        }

        private static void GenerateLoginProblemDetails()
        {
            // 400 - BadRequest

            var badRequestProblem =
                BundleRefitProblemDetails(BadRequest,
                    title: LoginFailedTitle,
                    detail : "Invalid fields: Username, Password",
                    userMessages: LoginFailedUserMessages);

            FancyLogger!.LogProblemDetails(badRequestProblem);

            // 403 - Forbidden

            var usernameNotFoundProblem =
                BundleRefitProblemDetails(Forbidden,
                    title: LoginFailedTitle,
                    detail: "Username and/or Password do not match",
                    userMessages: LoginFailedUserMessages);

            FancyLogger.LogProblemDetails(usernameNotFoundProblem);
        }
    }
}
