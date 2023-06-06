using System;
using System.Diagnostics;
using XamarinFiles.FancyLogger;
using XamarinFiles.FancyLogger.Extensions;
using static System.Net.HttpStatusCode;
using static XamarinFiles.PdHelpers.Refit.Bundlers;

namespace XamarinFiles.PdHelpers.Tests.Smoke
{
    internal class Program
    {
        #region Fields

        private const string LoginFailedTitle = "Invalid Credentials";

        private static readonly string[] LoginFailedUserMessages =
        {
            "Please check your Username and Password and try again"
        };

        #endregion

        #region Services

        private static FancyLoggerService? LoggerService { get; }

        private static AssemblyLoggerService? AssemblyLoggerService { get; }

        #endregion

        #region Constructor

        static Program()
        {
            try
            {
                LoggerService = new FancyLoggerService();
                AssemblyLoggerService = new AssemblyLoggerService(LoggerService);
            }
            catch (Exception exception)
            {
                if (LoggerService is not null)
                {
                    LoggerService.LogExceptionRouter(exception);
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
                /* WARNING - Requires shared project from another repo \/\/\/\/\/ */

                AssemblyLoggerService?.LogAssemblies(showCultureInfo: false);

                LoggerService?.LogHeader("PdHelpers Tests");

                /* /\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\ */

                if (LoggerService is null)
                {
                    return;
                }

                GenerateGeneralProblemDetails();
                GenerateJsonProblemDetails();
                GenerateLoginProblemDetails();
            }
            catch (Exception exception)
            {
                LoggerService?.LogExceptionRouter(exception);
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

            LoggerService!.LogProblemDetails(badRequestProblem);

            // 403 - Forbidden

            var usernameNotFoundProblem =
                BundleRefitProblemDetails(Forbidden,
                    title: LoginFailedTitle,
                    detail: "Username and/or Password do not match",
                    userMessages: LoginFailedUserMessages);

            LoggerService.LogProblemDetails(usernameNotFoundProblem);
        }
    }
}
