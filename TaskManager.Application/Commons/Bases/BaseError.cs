using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.Commons.Bases
{
    [ExcludeFromCodeCoverage]
    public class BaseError
    {
        public string? PropertyMessage { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
