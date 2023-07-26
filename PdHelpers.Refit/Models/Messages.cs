using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Messages
    {
        #region Smart Constructor

        private Messages(string[]? developerMessages,
            string[]? userMessages,
            ExceptionMessages? exceptionMessages)
        {
            if (developerMessages != null)
                DeveloperMessages = developerMessages;

            if (userMessages != null)
                UserMessages = userMessages;

            if (exceptionMessages != null)
                ExceptionMessages = exceptionMessages;
        }

        public static Messages? Create(
            string[]? developerMessages = null,
            string[]? userMessages = null,
            Exception? exception = null)
        {
            var exceptionMessages =
                ExceptionMessages.Create(exception);

            var messages =
                Create(developerMessages, userMessages, exceptionMessages);

            return messages;
        }

        public static Messages? Create(
            string[]? developerMessages = null,
            string[]? userMessages = null,
            ExceptionMessages? exceptionMessages = null)
        {
            if (developerMessages == null
                && userMessages == null
                && exceptionMessages == null)
                return null;

            var messages =
                new Messages(developerMessages, userMessages, exceptionMessages);

            return messages;
        }

        #endregion

        #region API/App Properties

        [JsonPropertyName("developerMessages")]
        public string[]? DeveloperMessages { get; }

        [JsonPropertyName("userMessages")]
        public string[]? UserMessages { get; }

        #endregion

        #region Exception Properties

        [JsonPropertyName("exceptionMessages")]
        public ExceptionMessages? ExceptionMessages { get; }

        #endregion
    }
}
