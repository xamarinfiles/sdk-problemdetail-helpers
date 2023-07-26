using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using static System.String;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ExceptionMessages
    {
        #region Smart Constructor

        private ExceptionMessages(string outerExceptionMessage,
            string? innerExceptionMessage)
        {
            OuterExceptionMessage = outerExceptionMessage;

            if (!IsNullOrWhiteSpace(innerExceptionMessage))
                InnerExceptionMessage = innerExceptionMessage;
        }

        public static ExceptionMessages? Create(Exception? exception)
        {
            if (exception is null)
                return null;

            var exceptionMessages =
                new ExceptionMessages(exception.Message,
                    exception.InnerException?.Message);

            return exceptionMessages;
        }

        #endregion

        #region Properties

        [JsonPropertyName("outerExceptionMessage")]
        public string OuterExceptionMessage { get; }

        [JsonPropertyName("innerExceptionMessage")]
        public string? InnerExceptionMessage { get; }

        // TODO Break out other exception messages when add logic to pull them
        //[JsonPropertyName("otherExceptionMessages")]
        //public IDictionary<string, string> OtherExceptionMessages { get; } =
        //    new Dictionary<string, string>();

        #endregion
    }
}
