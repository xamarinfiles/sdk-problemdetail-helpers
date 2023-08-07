using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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

        [JsonPropertyName("outerException")]
        public string OuterExceptionMessage { get; }

        [JsonPropertyName("innerException")]
        public string? InnerExceptionMessage { get; }

        // TODO Break out other exception messages when add logic to pull them
        //[JsonPropertyName("otherExceptions")]
        //public IDictionary<string, string> OtherExceptions { get; } =
        //    new Dictionary<string, string>();

        #endregion
    }
}
