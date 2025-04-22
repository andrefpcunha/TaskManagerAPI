using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.Commons.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ValidationExceptionCustom : Exception
    {
        public IEnumerable<BaseError> Errors { get; }

        public ValidationExceptionCustom()
            : base("One or more validation failures have occured.")
        {
            Errors = new List<BaseError>();
        }

        public ValidationExceptionCustom(IEnumerable<BaseError> errors)
            : this()
        {
            Errors = errors;
        }
    }
}
