using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    // TODO Add other properties like StackTrace?
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ExceptionDetails
    {
        #region Smart Constructor

        private ExceptionDetails(string? assembly,
            string? method,
            ExceptionMessages? messages)
        {
            if (assembly is null
                && method is null
                && messages is null)
                return;

            Assembly = assembly;
            Method = method;
            Messages = messages;
        }

        public static ExceptionDetails?
            Create(Exception? exception)
        {
            if (exception is null)
                return null;

            var messages = ExceptionMessages.Create(exception);

            var exceptionDetails =
                new ExceptionDetails(exception.Source,
                    exception.TargetSite?.Name,
                    messages);

            return exceptionDetails;
        }

        #endregion

        #region Exception Properties

        [JsonPropertyName("assembly")]
        public string? Assembly { get; }

        [JsonPropertyName("method")]
        public string? Method { get; }

        [JsonPropertyName("messages")]
        public ExceptionMessages? Messages { get; }

        #endregion
    }
}
