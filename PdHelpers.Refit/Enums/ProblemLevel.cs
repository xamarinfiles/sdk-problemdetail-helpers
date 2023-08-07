using System.Diagnostics.CodeAnalysis;

namespace XamarinFiles.PdHelpers.Refit.Enums
{
    // TODO Better way to implement w/o assuming default value for true/false?
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum ProblemLevel
    {
        Error,
        Warning
    }
}
