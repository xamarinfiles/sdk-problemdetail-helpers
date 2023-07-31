using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace XamarinFiles.PdHelpers.Refit.Models
{
    // NOTE ExceptionMessages were consolidated into ExceptionDetails
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class OtherMessages
    {
        #region Smart Constructor

        private OtherMessages(IList<string>? developerMessages,
            IList<string>? userMessages)
        {
            DeveloperMessages = developerMessages;
            UserMessages = userMessages;
        }

        public static OtherMessages? Create(
            IList<string>? developerMessages = null,
            IList<string>? userMessages = null)
        {
            if (developerMessages == null
                && userMessages == null)
                return null;

            var messages = new OtherMessages(developerMessages, userMessages);

            return messages;
        }

        #endregion

        #region API/App Properties

        [JsonPropertyName("developerMessages")]
        public IList<string>? DeveloperMessages { get; }

        [JsonPropertyName("userMessages")]
        public IList<string>? UserMessages { get; }

        #endregion
    }
}
