using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.Commons.Extensions
{
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
